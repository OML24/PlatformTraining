using Microsoft.EntityFrameworkCore;
using ZU.Training.Application.Training;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;
using ZU.Training.Infrastructure.Data;

namespace ZU.Training.Infrastructure.Repositories;

public sealed class EfHistoricalCourseStore(TrainingDbContext db) : IHistoricalCourseStore
{
    public Task<TrainingTrack?> TrackAsync(Guid id, CancellationToken ct)
        => db.TrainingTracks.SingleOrDefaultAsync(x => x.Id == id, ct);

    public Task<EvaluationFormVersion?> LatestEvaluationVersionAsync(CancellationToken ct)
        => db.EvaluationFormVersions.OrderByDescending(x => x.Version).FirstOrDefaultAsync(ct);

    public Task<UserAccount?> UserAsync(Guid id, CancellationToken ct)
        => db.UserAccounts.SingleOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<EvaluationQuestion>> EvaluationQuestionsAsync(Guid formVersionId, CancellationToken ct)
        => await db.EvaluationQuestions.Where(x => x.FormVersionId == formVersionId).ToListAsync(ct);

    public Task<EvaluationOption?> FirstEvaluationOptionAsync(Guid questionId, CancellationToken ct)
        => db.EvaluationOptions.Where(x => x.QuestionId == questionId).OrderBy(x => x.Position).FirstOrDefaultAsync(ct);

    public void AddCourse(Course course) => db.Courses.Add(course);
    public void AddLecture(Lecture lecture) => db.Lectures.Add(lecture);
    public void AddExam(Exam exam) => db.Exams.Add(exam);
    public void AddEnrollment(CourseEnrollment enrollment) => db.CourseEnrollments.Add(enrollment);
    public void AddAttendance(AttendanceRecord attendance) => db.AttendanceRecords.Add(attendance);
    public void AddExamResult(ExamResult examResult) => db.ExamResults.Add(examResult);
    public void AddEvaluationResponse(EvaluationResponse response) => db.EvaluationResponses.Add(response);
    public void AddEvaluationAnswer(EvaluationAnswer answer) => db.EvaluationAnswers.Add(answer);
    public void AddImpactSubmission(ImpactSubmission submission) => db.ImpactSubmissions.Add(submission);

    public Task SaveAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}
