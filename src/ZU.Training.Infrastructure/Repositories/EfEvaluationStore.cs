using Microsoft.EntityFrameworkCore;
using ZU.Training.Application.Training;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;
using ZU.Training.Infrastructure.Data;

namespace ZU.Training.Infrastructure.Repositories;

public sealed class EfEvaluationStore(TrainingDbContext db) : IEvaluationStore
{
    public Task<UserAccount?> UserAsync(Guid id, CancellationToken ct)
        => db.UserAccounts.SingleOrDefaultAsync(x => x.Id == id, ct);
    public Task<Course?> CourseAsync(Guid id, CancellationToken ct)
        => db.Courses.SingleOrDefaultAsync(x => x.Id == id && x.IsActive, ct);
    public Task<CourseEnrollment?> EnrollmentAsync(Guid courseId, Guid userId, CancellationToken ct)
        => db.CourseEnrollments.SingleOrDefaultAsync(x => x.CourseId == courseId && x.UserId == userId, ct);
    public Task<EvaluationFormVersion?> LatestVersionAsync(CancellationToken ct)
        => db.EvaluationFormVersions.OrderByDescending(x => x.Version).FirstOrDefaultAsync(ct);
    public Task<EvaluationFormVersion?> VersionAsync(Guid id, CancellationToken ct)
        => db.EvaluationFormVersions.SingleOrDefaultAsync(x => x.Id == id, ct);
    public async Task<IReadOnlyList<EvaluationFormVersion>> VersionsAsync(CancellationToken ct)
        => await db.EvaluationFormVersions.AsNoTracking().Where(x => x.IsActive)
            .OrderByDescending(x => x.Version).ToListAsync(ct);
    public async Task<IReadOnlyList<EvaluationQuestion>> QuestionsAsync(Guid versionId, CancellationToken ct)
        => await db.EvaluationQuestions.Where(x => x.FormVersionId == versionId && x.IsActive)
            .OrderBy(x => x.Position).ToListAsync(ct);
    public async Task<IReadOnlyList<EvaluationOption>> OptionsAsync(Guid questionId, CancellationToken ct)
        => await db.EvaluationOptions.Where(x => x.QuestionId == questionId && x.IsActive)
            .OrderBy(x => x.Position).ToListAsync(ct);
    public Task<EvaluationResponse?> ResponseAsync(Guid versionId, Guid enrollmentId, CancellationToken ct)
        => db.EvaluationResponses.SingleOrDefaultAsync(x => x.FormVersionId == versionId &&
            x.EnrollmentId == enrollmentId && x.IsActive, ct);
    public async Task<IReadOnlyList<EvaluationAnswer>> AnswersAsync(Guid courseId, Guid versionId, CancellationToken ct)
        => await (from answer in db.EvaluationAnswers
                  join response in db.EvaluationResponses on answer.ResponseId equals response.Id
                  join enrollment in db.CourseEnrollments on response.EnrollmentId equals enrollment.Id
                  where response.FormVersionId == versionId && response.IsActive && answer.IsActive &&
                        enrollment.CourseId == courseId && enrollment.IsActive
                  select answer).ToListAsync(ct);
    public async Task<IReadOnlyList<Course>> OpenCoursesAsync(DateTimeOffset now, CancellationToken ct)
        => await db.Courses.Where(x => x.IsActive && !x.IsArchived && x.EvaluationDeadlineUtc > now &&
            x.Status != CourseStatus.Cancelled && x.Status != CourseStatus.Rejected).ToListAsync(ct);
    public Task<int> AttendanceCountAsync(Guid courseId, Guid enrollmentId, CancellationToken ct)
        => (from record in db.AttendanceRecords
            join lecture in db.Lectures on record.LectureId equals lecture.Id
            where lecture.CourseId == courseId && lecture.IsActive && record.EnrollmentId == enrollmentId &&
                  record.IsActive && record.IsPresent
            select record).CountAsync(ct);
    public Task<int> LectureCountAsync(Guid courseId, CancellationToken ct)
        => db.Lectures.CountAsync(x => x.CourseId == courseId && x.IsActive, ct);
    public async Task<ExamOutcome> ExamOutcomeAsync(Guid courseId, Guid enrollmentId, CancellationToken ct)
    {
        var result = await (from exam in db.Exams
                            join row in db.ExamResults on exam.Id equals row.ExamId
                            where exam.CourseId == courseId && exam.IsActive && row.EnrollmentId == enrollmentId && row.IsActive
                            select row.Outcome).FirstOrDefaultAsync(ct);
        return result;
    }
    public void AddVersion(EvaluationFormVersion x) => db.EvaluationFormVersions.Add(x);
    public void AddQuestion(EvaluationQuestion x) => db.EvaluationQuestions.Add(x);
    public void AddOption(EvaluationOption x) => db.EvaluationOptions.Add(x);
    public void AddResponse(EvaluationResponse x) => db.EvaluationResponses.Add(x);
    public void AddAnswer(EvaluationAnswer x) => db.EvaluationAnswers.Add(x);
    public Task SaveAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}
