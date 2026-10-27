namespace ZU.Training.Domain.Training;

public static class AttendancePolicy
{
    public const decimal MinimumRate = 0.75m;

    public static int RequiredLectureCount(int totalLectures)
    {
        if (totalLectures < 1)
            throw new ArgumentOutOfRangeException(nameof(totalLectures), "A course needs at least one lecture.");

        return (int)Math.Ceiling(totalLectures * MinimumRate);
    }

    public static bool IsSatisfied(int attendedLectures, int totalLectures)
    {
        if (attendedLectures < 0 || attendedLectures > totalLectures)
            throw new ArgumentOutOfRangeException(nameof(attendedLectures));

        return attendedLectures >= RequiredLectureCount(totalLectures);
    }
}
