using Microsoft.EntityFrameworkCore;
using ZU.Training.Application.Administration;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;
using ZU.Training.Infrastructure.Data;

namespace ZU.Training.Infrastructure.Repositories;

public sealed class EfAnalyticsStore(TrainingDbContext db) : IAnalyticsStore
{
    public async Task<IReadOnlyList<Course>> CoursesAsync(CancellationToken ct)
        => await db.Courses.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<CourseEnrollment>> ActiveEnrollmentsAsync(CancellationToken ct)
        => await db.CourseEnrollments.AsNoTracking().Where(x => x.IsActive).ToListAsync(ct);

    public async Task<IReadOnlyList<Certificate>> ActiveCertificatesAsync(CancellationToken ct)
        => await db.Certificates.AsNoTracking().Where(x => x.IsActive).ToListAsync(ct);

    public async Task<IReadOnlyList<Faculty>> ActiveFacultiesAsync(CancellationToken ct)
        => await db.Faculties.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.NameAr).ToListAsync(ct);

    public async Task<IReadOnlyList<TrainingTrack>> ActiveTracksAsync(CancellationToken ct)
        => await db.TrainingTracks.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.NameAr).ToListAsync(ct);

    public async Task<IReadOnlyList<CourseCancellationRequest>> ApprovedCancellationsAsync(CancellationToken ct)
        => await db.CourseCancellationRequests.AsNoTracking()
            .Where(x => x.Status == QualityRequestStatus.Approved).ToListAsync(ct);

    public async Task<IReadOnlyList<ExamResult>> ActiveExamResultsAsync(CancellationToken ct)
        => await db.ExamResults.AsNoTracking().Where(x => x.IsActive).ToListAsync(ct);

    public Task<Course?> CourseAsync(Guid courseId, CancellationToken ct)
        => db.Courses.AsNoTracking().SingleOrDefaultAsync(x => x.Id == courseId, ct);

    public async Task<IReadOnlyList<Lecture>> CourseLecturesAsync(Guid courseId, CancellationToken ct)
        => await db.Lectures.AsNoTracking()
            .Where(x => x.CourseId == courseId && x.IsActive)
            .OrderBy(x => x.SequenceNumber)
            .ToListAsync(ct);

    public Task<int> CourseEnrollmentsCountAsync(Guid courseId, CancellationToken ct)
        => db.CourseEnrollments.AsNoTracking().CountAsync(x => x.CourseId == courseId && x.IsActive, ct);

    public Task<int> LectureAttendanceCountAsync(Guid lectureId, CancellationToken ct)
        => db.AttendanceRecords.AsNoTracking().CountAsync(x => x.LectureId == lectureId && x.IsPresent && x.IsActive, ct);
}
