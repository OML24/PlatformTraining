using Microsoft.EntityFrameworkCore;
using ZU.Training.Application.Administration;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;
using ZU.Training.Infrastructure.Data;

namespace ZU.Training.Infrastructure.Repositories;

public sealed class EfUserProfileStore(TrainingDbContext db) : IUserProfileStore
{
    public Task<UserAccount?> UserAsync(Guid id, CancellationToken ct)
        => db.UserAccounts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);

    public Task<Faculty?> FacultyAsync(Guid id, CancellationToken ct)
        => db.Faculties.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);

    public Task<Department?> DepartmentAsync(Guid id, CancellationToken ct)
        => db.Departments.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<string>> UserRoleKeysAsync(Guid userId, CancellationToken ct)
        => await (from ur in db.UserRoles.AsNoTracking()
                  join r in db.Roles.AsNoTracking() on ur.RoleId equals r.Id
                  where ur.UserId == userId && ur.IsActive
                  select r.Key).ToListAsync(ct);

    public async Task<IReadOnlyList<CourseEnrollment>> UserActiveEnrollmentsAsync(Guid userId, CancellationToken ct)
        => await db.CourseEnrollments.AsNoTracking()
            .Where(x => x.UserId == userId && x.IsActive)
            .OrderByDescending(x => x.EnrolledAtUtc)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Course>> CoursesByIdsAsync(IReadOnlyList<Guid> courseIds, CancellationToken ct)
        => await db.Courses.AsNoTracking()
            .Where(x => courseIds.Contains(x.Id))
            .ToListAsync(ct);
}
