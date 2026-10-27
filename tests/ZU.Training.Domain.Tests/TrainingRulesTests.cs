using ZU.Training.Domain.Training;

namespace ZU.Training.Domain.Tests;

public sealed class TrainingRulesTests
{
    [Theory]
    [InlineData(10, 8)]
    [InlineData(5, 4)]
    [InlineData(4, 3)]
    [InlineData(1, 1)]
    public void RequiredAttendanceRoundsUp(int lectures, int expected)
        => Assert.Equal(expected, AttendancePolicy.RequiredLectureCount(lectures));

    [Fact]
    public void SevenOfTenLecturesDoesNotMeetAttendance()
        => Assert.False(AttendancePolicy.IsSatisfied(7, 10));

    [Fact]
    public void CodeExpiresAtFiveMinuteBoundary()
    {
        var activated = new DateTimeOffset(2026, 10, 26, 9, 0, 0, TimeSpan.Zero);
        Assert.True(AttendanceCodeWindow.IsOpen(activated, activated.AddMinutes(4).AddSeconds(59)));
        Assert.False(AttendanceCodeWindow.IsOpen(activated, activated.AddMinutes(5)));
    }

    [Fact]
    public void CourseWithoutExamCanIssueWhenOtherRequirementsAreMet()
    {
        var result = CertificateEligibility.Evaluate(new(
            CourseEnded: true,
            TotalLectures: 4,
            AttendedLectures: 3,
            ExamRequired: false,
            ExamOutcome: ExamOutcome.NotRecorded,
            EvaluationCompleted: true,
            ImpactApproved: true));

        Assert.True(result.IsEligible);
    }

    [Fact]
    public void MissingExamResultDoesNotCountAsFailureOrSuccess()
    {
        var result = CertificateEligibility.Evaluate(new(
            CourseEnded: true,
            TotalLectures: 4,
            AttendedLectures: 4,
            ExamRequired: true,
            ExamOutcome: ExamOutcome.NotRecorded,
            EvaluationCompleted: true,
            ImpactApproved: true));

        Assert.False(result.IsEligible);
        Assert.Contains(CertificateIneligibilityReason.ExamNotPassed, result.Reasons);
    }

    [Fact]
    public void ACourseCannotIssueCertificatesWithoutLectures()
    {
        var result = CertificateEligibility.Evaluate(new(
            CourseEnded: true,
            TotalLectures: 0,
            AttendedLectures: 0,
            ExamRequired: false,
            ExamOutcome: ExamOutcome.NotRecorded,
            EvaluationCompleted: true,
            ImpactApproved: true));

        Assert.False(result.IsEligible);
        Assert.Contains(CertificateIneligibilityReason.NoLectures, result.Reasons);
    }
}
