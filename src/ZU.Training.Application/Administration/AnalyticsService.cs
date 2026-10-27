using ZU.Training.Application.Security;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Administration;

public sealed class AnalyticsService(IAnalyticsStore store)
{
    public async Task<UniversityAnalyticsSummary> GetDashboardAnalyticsAsync(Guid actorId, CancellationToken ct)
    {
        var courses = await store.CoursesAsync(ct);
        var enrollments = await store.ActiveEnrollmentsAsync(ct);
        var certificates = await store.ActiveCertificatesAsync(ct);
        var faculties = await store.ActiveFacultiesAsync(ct);
        var tracks = await store.ActiveTracksAsync(ct);
        var cancellations = await store.ApprovedCancellationsAsync(ct);
        var examResults = await store.ActiveExamResultsAsync(ct);

        var totalCourses = courses.Count;
        var activeCourses = courses.Count(x => x.Status is CourseStatus.Approved or CourseStatus.InProgress);
        var endedCourses = courses.Count(x => x.Status == CourseStatus.Ended);
        var cancelledCourses = courses.Count(x => x.Status == CourseStatus.Cancelled);
        var totalHours = courses.Where(x => x.Status == CourseStatus.Ended).Sum(x => x.PlannedHours);

        var totalEnrollments = enrollments.Count;
        var totalTrainees = enrollments.Select(x => x.UserId).Distinct().Count();

        var totalCertificatesIssued = certificates.Count(x => x.Status == CertificateStatus.Valid);
        var totalCertificatesRevoked = certificates.Count(x => x.Status == CertificateStatus.Revoked);

        var passRate = 0.0;
        if (examResults.Count > 0)
        {
            var passedCount = examResults.Count(x => x.Outcome == ExamOutcome.Passed);
            passRate = Math.Round((double)passedCount / examResults.Count * 100, 1);
        }

        // Faculty breakdown
        var facultyList = new List<FacultyAnalyticsSummary>();
        foreach (var f in faculties)
        {
            var fCourses = courses.Where(x => x.FacultyId == f.Id).ToList();
            var fCourseIds = fCourses.Select(x => x.Id).ToHashSet();
            var fEnrollments = enrollments.Count(x => fCourseIds.Contains(x.CourseId));
            var fCerts = certificates.Count(x => fCourseIds.Contains(x.CourseId) && x.Status == CertificateStatus.Valid);

            facultyList.Add(new FacultyAnalyticsSummary(
                f.Id, f.NameAr, f.NameEn, fCourses.Count, fEnrollments, fCerts));
        }

        // Track breakdown
        var trackList = new List<TrackAnalyticsSummary>();
        foreach (var t in tracks)
        {
            var tCourses = courses.Where(x => x.TrainingTrackId == t.Id).ToList();
            var tHours = tCourses.Sum(x => x.PlannedHours);
            trackList.Add(new TrackAnalyticsSummary(t.Id, t.NameAr, t.NameEn, tCourses.Count, tHours));
        }

        // Cancelled report
        var cancelList = new List<CancelledCourseSummary>();
        foreach (var c in cancellations)
        {
            var course = courses.FirstOrDefault(x => x.Id == c.CourseId);
            var fName = course?.FacultyId.HasValue == true
                ? faculties.FirstOrDefault(x => x.Id == course.FacultyId.Value)?.NameAr
                : "عامة";

            cancelList.Add(new CancelledCourseSummary(
                c.CourseId,
                course?.TitleAr ?? "دورة ملغاة",
                course?.TitleEn,
                fName,
                c.ReasonAr,
                c.ReviewedAtUtc,
                c.OfficialDocumentOriginalName
            ));
        }

        return new UniversityAnalyticsSummary(
            totalCourses,
            activeCourses,
            endedCourses,
            cancelledCourses,
            totalHours,
            totalTrainees,
            totalEnrollments,
            totalCertificatesIssued,
            totalCertificatesRevoked,
            passRate,
            facultyList,
            trackList,
            cancelList
        );
    }

    public async Task<IReadOnlyList<CourseSessionAnalytics>> GetCourseSessionsAnalyticsAsync(
        Guid courseId, CancellationToken ct)
    {
        var course = await store.CourseAsync(courseId, ct)
            ?? throw new KeyNotFoundException("Course not found.");

        var lectures = await store.CourseLecturesAsync(courseId, ct);
        var totalEnrolled = await store.CourseEnrollmentsCountAsync(courseId, ct);

        var result = new List<CourseSessionAnalytics>();
        foreach (var lec in lectures)
        {
            var attended = await store.LectureAttendanceCountAsync(lec.Id, ct);
            var pct = totalEnrolled > 0 ? Math.Round((double)attended / totalEnrolled * 100, 1) : 0;
            result.Add(new CourseSessionAnalytics(
                course.Id, course.TitleAr, lec.SequenceNumber, lec.TitleAr, attended, totalEnrolled, pct));
        }

        return result;
    }
}
