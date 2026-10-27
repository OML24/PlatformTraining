namespace ZU.Training.Domain.Training;

public static class AttendanceCodeWindow
{
    public static readonly TimeSpan Duration = TimeSpan.FromMinutes(5);

    public static bool IsOpen(DateTimeOffset activatedAtUtc, DateTimeOffset checkedAtUtc)
        => checkedAtUtc >= activatedAtUtc && checkedAtUtc < activatedAtUtc.Add(Duration);
}
