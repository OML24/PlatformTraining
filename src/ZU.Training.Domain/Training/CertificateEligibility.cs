namespace ZU.Training.Domain.Training;

public sealed record CertificateEligibilityInput(
    bool CourseEnded,
    int TotalLectures,
    int AttendedLectures,
    bool ExamRequired,
    ExamOutcome ExamOutcome,
    bool EvaluationCompleted,
    bool ImpactApproved);

public enum ExamOutcome
{
    NotRecorded = 0,
    Passed = 1,
    Failed = 2
}

public enum CertificateIneligibilityReason
{
    CourseNotEnded,
    NoLectures,
    InsufficientAttendance,
    ExamNotPassed,
    EvaluationMissing,
    ImpactNotApproved
}

public sealed record CertificateEligibilityResult(
    bool IsEligible,
    IReadOnlyList<CertificateIneligibilityReason> Reasons);

public static class CertificateEligibility
{
    public static CertificateEligibilityResult Evaluate(CertificateEligibilityInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (input.TotalLectures < 0)
            throw new ArgumentOutOfRangeException(nameof(input.TotalLectures));
        if (input.AttendedLectures < 0 || input.AttendedLectures > input.TotalLectures)
            throw new ArgumentOutOfRangeException(nameof(input.AttendedLectures));

        var reasons = new List<CertificateIneligibilityReason>();

        if (!input.CourseEnded)
            reasons.Add(CertificateIneligibilityReason.CourseNotEnded);
        if (input.TotalLectures == 0)
            reasons.Add(CertificateIneligibilityReason.NoLectures);
        else if (!AttendancePolicy.IsSatisfied(input.AttendedLectures, input.TotalLectures))
            reasons.Add(CertificateIneligibilityReason.InsufficientAttendance);
        if (input.ExamRequired && input.ExamOutcome != ExamOutcome.Passed)
            reasons.Add(CertificateIneligibilityReason.ExamNotPassed);
        if (!input.EvaluationCompleted)
            reasons.Add(CertificateIneligibilityReason.EvaluationMissing);
        if (!input.ImpactApproved)
            reasons.Add(CertificateIneligibilityReason.ImpactNotApproved);

        return new CertificateEligibilityResult(reasons.Count == 0, reasons);
    }
}
