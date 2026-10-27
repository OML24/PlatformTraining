using ZU.Training.Domain.Common;

namespace ZU.Training.Domain.Training;

public sealed class EvaluationFormVersion : Entity
{
    public int Version { get; set; }
    public DateTimeOffset PublishedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public Guid PublishedByUserId { get; set; }
}

public sealed class EvaluationQuestion : Entity
{
    public Guid FormVersionId { get; set; }
    public int Position { get; set; }
    public required string TextAr { get; set; }
    public string? TextEn { get; set; }
}

public sealed class EvaluationOption : Entity
{
    public Guid QuestionId { get; set; }
    public int Position { get; set; }
    public required string TextAr { get; set; }
    public string? TextEn { get; set; }
}

public sealed class EvaluationResponse : Entity
{
    public Guid FormVersionId { get; set; }
    public Guid EnrollmentId { get; set; }
    public DateTimeOffset SubmittedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class EvaluationAnswer : Entity
{
    public Guid ResponseId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid OptionId { get; set; }
}

public enum ImpactStatus { Pending = 0, Returned = 1, Approved = 2 }

public sealed class ImpactSubmission : Entity
{
    public Guid EnrollmentId { get; set; }
    public int Version { get; set; }
    public required string OriginalName { get; set; }
    public required string StorageKey { get; set; }
    public required string Sha256 { get; set; }
    public ImpactStatus Status { get; set; } = ImpactStatus.Pending;
    public string? ReviewNoteAr { get; set; }
    public Guid? ReviewedByUserId { get; set; }
    public DateTimeOffset? ReviewedAtUtc { get; set; }
}
