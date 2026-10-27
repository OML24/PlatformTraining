using ZU.Training.Domain.Common;

namespace ZU.Training.Domain.Training;

public enum QualityRequestStatus { Submitted = 0, RevisionRequested = 1, Approved = 2, Rejected = 3 }

public sealed class TrainingNeed : Entity
{
    public Guid FacultyId { get; set; }
    public Guid SubmittedByUserId { get; set; }
    public required string TitleAr { get; set; }
    public string? TitleEn { get; set; }
    public required string DescriptionAr { get; set; }
    public string? DescriptionEn { get; set; }
    public QualityRequestStatus Status { get; set; } = QualityRequestStatus.Submitted;
    public string? ReviewNoteAr { get; set; }
    public Guid? ReviewedByUserId { get; set; }
    public DateTimeOffset? ReviewedAtUtc { get; set; }
    public required string OfficialDocumentStorageKey { get; set; }
    public required string OfficialDocumentOriginalName { get; set; }
    public required string OfficialDocumentSha256 { get; set; }
}

public sealed class CourseCancellationRequest : Entity
{
    public Guid CourseId { get; set; }
    public Guid RequestedByUserId { get; set; }
    public required string ReasonAr { get; set; }
    public QualityRequestStatus Status { get; set; } = QualityRequestStatus.Submitted;
    public string? ReviewNoteAr { get; set; }
    public Guid? ReviewedByUserId { get; set; }
    public DateTimeOffset? ReviewedAtUtc { get; set; }
    public required string OfficialDocumentStorageKey { get; set; }
    public required string OfficialDocumentOriginalName { get; set; }
    public required string OfficialDocumentSha256 { get; set; }
}

public sealed class OfficialDocument : Entity
{
    public required string EntityType { get; set; }
    public Guid EntityId { get; set; }
    public required string Kind { get; set; }
    public required string OriginalName { get; set; }
    public required string StorageKey { get; set; }
    public required string Sha256 { get; set; }
    public Guid UploadedByUserId { get; set; }
}
