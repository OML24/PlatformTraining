using ZU.Training.Application.Training;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Administration;

public sealed record UserProfileCourseDto(
    Guid CourseId,
    string TitleAr,
    string? TitleEn,
    decimal PlannedHours,
    CourseStatus Status,
    DateTimeOffset EnrolledAtUtc,
    bool HasCertificate,
    string? CertificateReference
);

public sealed record UserProfileDto(
    Guid UserId,
    string LoginName,
    int? EmployeeId,
    string NameAr,
    string? NameEn,
    string? Email,
    bool IsExternalTrainer,
    string PreferredLanguage,
    string? FacultyNameAr,
    string? FacultyNameEn,
    string? DepartmentNameAr,
    string? DepartmentNameEn,
    IReadOnlyList<string> Roles,
    int CompletedCoursesCount,
    decimal TotalEarnedHours,
    IReadOnlyList<UserProfileCourseDto> Courses,
    IReadOnlyList<UserCertificateDto> Certificates
);

public interface IUserProfileStore
{
    Task<UserAccount?> UserAsync(Guid id, CancellationToken ct);
    Task<Faculty?> FacultyAsync(Guid id, CancellationToken ct);
    Task<Department?> DepartmentAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<string>> UserRoleKeysAsync(Guid userId, CancellationToken ct);
    Task<IReadOnlyList<CourseEnrollment>> UserActiveEnrollmentsAsync(Guid userId, CancellationToken ct);
    Task<IReadOnlyList<Course>> CoursesByIdsAsync(IReadOnlyList<Guid> courseIds, CancellationToken ct);
}
