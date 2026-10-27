using Microsoft.EntityFrameworkCore;
using ZU.Training.Application.Training;
using ZU.Training.Domain.Training;
using ZU.Training.Infrastructure.Data;

namespace ZU.Training.Infrastructure.Repositories;

public sealed class EfCertificateGateStore(TrainingDbContext db) : ICertificateGateStore
{
    public Task<Course?> CourseAsync(Guid id, CancellationToken ct)
        => db.Courses.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.IsActive, ct);
    public Task<CourseEnrollment?> EnrollmentAsync(Guid courseId, Guid userId, CancellationToken ct)
        => db.CourseEnrollments.AsNoTracking().SingleOrDefaultAsync(x => x.CourseId == courseId && x.UserId == userId, ct);
    public Task<int> LectureCountAsync(Guid courseId, CancellationToken ct)
        => db.Lectures.CountAsync(x => x.CourseId == courseId && x.IsActive, ct);
    public Task<int> AttendanceCountAsync(Guid courseId, Guid enrollmentId, CancellationToken ct)
        => (from attendance in db.AttendanceRecords
            join lecture in db.Lectures on attendance.LectureId equals lecture.Id
            where lecture.CourseId == courseId && lecture.IsActive && attendance.EnrollmentId == enrollmentId &&
                  attendance.IsActive && attendance.IsPresent
            select attendance).CountAsync(ct);
    public async Task<ExamOutcome> ExamOutcomeAsync(Guid courseId, Guid enrollmentId, CancellationToken ct)
        => await (from exam in db.Exams
                  join result in db.ExamResults on exam.Id equals result.ExamId
                  where exam.CourseId == courseId && exam.IsActive && result.EnrollmentId == enrollmentId && result.IsActive
                  select result.Outcome).FirstOrDefaultAsync(ct);
    public Task<bool> HasEvaluationAsync(Guid? versionId, Guid enrollmentId, CancellationToken ct)
        => versionId.HasValue
            ? db.EvaluationResponses.AnyAsync(x => x.FormVersionId == versionId &&
                x.EnrollmentId == enrollmentId && x.IsActive, ct)
            : Task.FromResult(false);
    public Task<bool> HasApprovedImpactAsync(Guid enrollmentId, CancellationToken ct)
        => db.ImpactSubmissions.AnyAsync(x => x.EnrollmentId == enrollmentId && x.IsActive &&
            x.Status == ImpactStatus.Approved, ct);
    public Task<Certificate?> CertificateAsync(Guid enrollmentId, CancellationToken ct)
        => db.Certificates.AsNoTracking().Where(x => x.EnrollmentId == enrollmentId && x.IsActive)
            .OrderByDescending(x => x.IssuedAtUtc).FirstOrDefaultAsync(ct);
}
