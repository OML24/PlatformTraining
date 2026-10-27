using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Training;

public sealed record EvaluationOptionInput(string TextAr, string? TextEn);
public sealed record EvaluationQuestionInput(string TextAr, string? TextEn,
    IReadOnlyList<EvaluationOptionInput> Options);
public sealed record PublishEvaluationInput(IReadOnlyList<EvaluationQuestionInput> Questions);
public sealed record EvaluationAnswerInput(Guid QuestionId, Guid OptionId);
public sealed record SubmitEvaluationInput(IReadOnlyList<EvaluationAnswerInput> Answers);
public sealed record EvaluationOptionView(Guid Id, string TextAr, string? TextEn);
public sealed record EvaluationQuestionView(Guid Id, string TextAr, string? TextEn,
    IReadOnlyList<EvaluationOptionView> Options);
public sealed record EvaluationFormView(Guid Id, int Version, bool AlreadySubmitted,
    IReadOnlyList<EvaluationQuestionView> Questions);
public sealed record EvaluationCount(Guid QuestionId, Guid OptionId, string QuestionAr,
    string? QuestionEn, string OptionAr, string? OptionEn, int Count);
public sealed record EvaluationVersionSummary(Guid Id, int Version, DateTimeOffset PublishedAtUtc);

public interface IEvaluationStore
{
    Task<UserAccount?> UserAsync(Guid id, CancellationToken ct);
    Task<Course?> CourseAsync(Guid id, CancellationToken ct);
    Task<CourseEnrollment?> EnrollmentAsync(Guid courseId, Guid userId, CancellationToken ct);
    Task<EvaluationFormVersion?> LatestVersionAsync(CancellationToken ct);
    Task<EvaluationFormVersion?> VersionAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<EvaluationFormVersion>> VersionsAsync(CancellationToken ct);
    Task<IReadOnlyList<EvaluationQuestion>> QuestionsAsync(Guid versionId, CancellationToken ct);
    Task<IReadOnlyList<EvaluationOption>> OptionsAsync(Guid questionId, CancellationToken ct);
    Task<EvaluationResponse?> ResponseAsync(Guid versionId, Guid enrollmentId, CancellationToken ct);
    Task<IReadOnlyList<EvaluationAnswer>> AnswersAsync(Guid courseId, Guid versionId, CancellationToken ct);
    Task<IReadOnlyList<Course>> OpenCoursesAsync(DateTimeOffset now, CancellationToken ct);
    Task<int> AttendanceCountAsync(Guid courseId, Guid enrollmentId, CancellationToken ct);
    Task<int> LectureCountAsync(Guid courseId, CancellationToken ct);
    Task<ExamOutcome> ExamOutcomeAsync(Guid courseId, Guid enrollmentId, CancellationToken ct);
    void AddVersion(EvaluationFormVersion version);
    void AddQuestion(EvaluationQuestion question);
    void AddOption(EvaluationOption option);
    void AddResponse(EvaluationResponse response);
    void AddAnswer(EvaluationAnswer answer);
    Task SaveAsync(CancellationToken ct);
}
