using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Administration;

public sealed record UniversityAnalyticsSummary(
    int TotalCourses,
    int ActiveCourses,
    int EndedCourses,
    int CancelledCourses,
    decimal TotalHoursDelivered,
    int TotalTrainees,
    int TotalEnrollments,
    int TotalCertificatesIssued,
    int TotalCertificatesRevoked,
    double ExamPassRatePercentage,
    IReadOnlyList<FacultyAnalyticsSummary> FacultyBreakdown,
    IReadOnlyList<TrackAnalyticsSummary> TrackBreakdown,
    IReadOnlyList<CancelledCourseSummary> CancelledCoursesReport
);

public sealed record FacultyAnalyticsSummary(
    Guid FacultyId,
    string FacultyNameAr,
    string? FacultyNameEn,
    int TotalCourses,
    int TotalEnrollments,
    int TotalCertificates
);

public sealed record TrackAnalyticsSummary(
    Guid TrackId,
    string TrackNameAr,
    string? TrackNameEn,
    int TotalCourses,
    decimal TotalHours
);

public sealed record CancelledCourseSummary(
    Guid CourseId,
    string TitleAr,
    string? TitleEn,
    string? FacultyNameAr,
    string ReasonAr,
    DateTimeOffset? CancelledAtUtc,
    string? OfficialDocumentOriginalName
);

public sealed record CourseSessionAnalytics(
    Guid CourseId,
    string CourseTitleAr,
    int LectureNumber,
    string LectureTitleAr,
    int AttendedCount,
    int TotalEnrolled,
    double AttendancePercentage
);

public interface IAnalyticsStore
{
    Task<IReadOnlyList<Course>> CoursesAsync(CancellationToken ct);
    Task<IReadOnlyList<CourseEnrollment>> ActiveEnrollmentsAsync(CancellationToken ct);
    Task<IReadOnlyList<Certificate>> ActiveCertificatesAsync(CancellationToken ct);
    Task<IReadOnlyList<Faculty>> ActiveFacultiesAsync(CancellationToken ct);
    Task<IReadOnlyList<TrainingTrack>> ActiveTracksAsync(CancellationToken ct);
    Task<IReadOnlyList<CourseCancellationRequest>> ApprovedCancellationsAsync(CancellationToken ct);
    Task<IReadOnlyList<ExamResult>> ActiveExamResultsAsync(CancellationToken ct);
    Task<Course?> CourseAsync(Guid courseId, CancellationToken ct);
    Task<IReadOnlyList<Lecture>> CourseLecturesAsync(Guid courseId, CancellationToken ct);
    Task<int> CourseEnrollmentsCountAsync(Guid courseId, CancellationToken ct);
    Task<int> LectureAttendanceCountAsync(Guid lectureId, CancellationToken ct);
}
