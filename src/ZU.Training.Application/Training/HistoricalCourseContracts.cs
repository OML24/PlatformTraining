using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Training;

public sealed record HistoricalParticipantInput(
    Guid UserId,
    IReadOnlyList<int> AttendedLectureIndices,
    ExamOutcome ExamOutcome,
    bool EvaluationCompleted,
    bool ImpactApproved
);

public sealed record HistoricalCourseInput(
    string TitleAr,
    string? TitleEn,
    Guid TrainingTrackId,
    string ObjectivesAr,
    string? ObjectivesEn,
    string TargetAudienceAr,
    string? TargetAudienceEn,
    DeliveryMode DeliveryMode,
    decimal PlannedHours,
    DateTimeOffset PlannedStartUtc,
    DateTimeOffset PlannedEndUtc,
    Guid? FacultyId,
    string ActualInstructorNameAr,
    string? ActualInstructorNameEn,
    bool HasExam,
    IReadOnlyList<string> LectureTitles,
    IReadOnlyList<HistoricalParticipantInput> Participants
);

public sealed record HistoricalCourseResult(
    Guid CourseId,
    int TotalParticipants,
    int CertificatesIssued
);

public interface IHistoricalCourseStore
{
    Task<TrainingTrack?> TrackAsync(Guid id, CancellationToken ct);
    Task<EvaluationFormVersion?> LatestEvaluationVersionAsync(CancellationToken ct);
    Task<UserAccount?> UserAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<EvaluationQuestion>> EvaluationQuestionsAsync(Guid formVersionId, CancellationToken ct);
    Task<EvaluationOption?> FirstEvaluationOptionAsync(Guid questionId, CancellationToken ct);
    void AddCourse(Course course);
    void AddLecture(Lecture lecture);
    void AddExam(Exam exam);
    void AddEnrollment(CourseEnrollment enrollment);
    void AddAttendance(AttendanceRecord attendance);
    void AddExamResult(ExamResult examResult);
    void AddEvaluationResponse(EvaluationResponse response);
    void AddEvaluationAnswer(EvaluationAnswer answer);
    void AddImpactSubmission(ImpactSubmission submission);
    Task SaveAsync(CancellationToken ct);
}
