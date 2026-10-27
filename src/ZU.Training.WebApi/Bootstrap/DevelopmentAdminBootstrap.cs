using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using ZU.Training.Application.Security;
using ZU.Training.Domain.Identity;
using ZU.Training.Infrastructure.Data;

namespace ZU.Training.WebApi.Bootstrap;

public static class DevelopmentAdminBootstrap
{
    public static async Task RunAsync(IServiceProvider services, string[] args)
    {
        var login = ReadArg(args, "--login");
        var name = ReadArg(args, "--name");
        var email = ReadArg(args, "--email");
        if (!int.TryParse(login, out var employeeId) || employeeId <= 0)
            throw new ArgumentException("--login must be a positive employee number.");

        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<TrainingDbContext>();
        var cipher = scope.ServiceProvider.GetRequiredService<ICredentialCipher>();
        if (await db.UserAccounts.AnyAsync(x => x.LoginName == login))
            throw new InvalidOperationException("This login already exists; bootstrap will not reset its password.");
        var adminRole = await db.Roles.SingleAsync(x => x.Key == "ADMIN");

        var password = Convert.ToBase64String(RandomNumberGenerator.GetBytes(24))
            .Replace('+', '-').Replace('/', '_').TrimEnd('=');
        var account = new UserAccount
        {
            LoginName = login,
            EmployeeId = employeeId,
            NameAr = name,
            Email = email,
            EncryptedInitialPassword = cipher.Encrypt(password),
            EncryptedCurrentPassword = cipher.Encrypt(password),
            FirstLoginCompleted = false,
            AccessOverrideAllow = true
        };
        db.UserAccounts.Add(account);
        db.UserRoles.Add(new UserRole { UserId = account.Id, RoleId = adminRole.Id });
        await db.SaveChangesAsync();

        Console.WriteLine($"Development admin created: {login}");
        Console.WriteLine($"Initial password (shown once): {password}");
    }

    private static string ReadArg(string[] args, string key)
    {
        var index = Array.IndexOf(args, key);
        if (index < 0 || index + 1 >= args.Length || string.IsNullOrWhiteSpace(args[index + 1]))
            throw new ArgumentException($"Missing {key} argument.");
        return args[index + 1];
    }
}
