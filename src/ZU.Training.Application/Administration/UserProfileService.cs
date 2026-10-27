using ZU.Training.Application.Training;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Administration;

public sealed class UserProfileService(IUserProfileStore store, CertificateService certificateService)
{
    public async Task<UserProfileDto> GetProfileAsync(Guid requesterId, Guid targetUserId, CancellationToken ct)
    {
        var user = await store.UserAsync(targetUserId, ct)
            ?? throw new KeyNotFoundException("User not found.");

        Faculty? faculty = null;
        if (user.FacultyId.HasValue)
            faculty = await store.FacultyAsync(user.FacultyId.Value, ct);

        Department? department = null;
        if (user.DepartmentId.HasValue)
            department = await store.DepartmentAsync(user.DepartmentId.Value, ct);

        var roles = await store.UserRoleKeysAsync(targetUserId, ct);
        var enrollments = await store.UserActiveEnrollmentsAsync(targetUserId, ct);

        var courseIds = enrollments.Select(x => x.CourseId).Distinct().ToList();
        var courses = (await store.CoursesByIdsAsync(courseIds, ct)).ToDictionary(x => x.Id);

        var certificates = await certificateService.GetUserPublicCertificatesAsync(requesterId, targetUserId, ct);
        var certByCourseId = certificates.ToDictionary(x => x.CourseId);

        var userCourses = new List<UserProfileCourseDto>();
        decimal totalEarnedHours = 0;
        var completedCount = 0;

        foreach (var en in enrollments)
        {
            if (!courses.TryGetValue(en.CourseId, out var c)) continue;

            var hasCert = certByCourseId.TryGetValue(c.Id, out var cert);
            if (hasCert && cert?.Status == CertificateStatus.Valid)
            {
                completedCount++;
                totalEarnedHours += c.PlannedHours;
            }

            userCourses.Add(new UserProfileCourseDto(
                c.Id,
                c.TitleAr,
                c.TitleEn,
                c.PlannedHours,
                c.Status,
                en.EnrolledAtUtc,
                hasCert,
                cert?.ReferenceNumber
            ));
        }

        return new UserProfileDto(
            user.Id,
            user.LoginName,
            user.EmployeeId,
            user.NameAr,
            user.NameEn,
            user.Email,
            user.IsExternalTrainer,
            user.PreferredLanguage,
            faculty?.NameAr,
            faculty?.NameEn,
            department?.NameAr,
            department?.NameEn,
            roles,
            completedCount,
            totalEarnedHours,
            userCourses,
            certificates
        );
    }
}
