using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZU.Training.Application.Training;
using ZU.Training.Domain.Training;

namespace ZU.Training.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/courses")]
public sealed class CoursesController(CourseWorkflowService workflow, CertificateGateService certificateGate) : ControllerBase
{
    [HttpGet]
    public Task<IActionResult> List(CancellationToken ct)
        => Execute(() => workflow.ListAsync(CurrentUserId(), ct));

    [HttpGet("tracks")]
    public Task<IActionResult> Tracks(CancellationToken ct)
        => Execute(() => workflow.TracksAsync(CurrentUserId(), ct));

    [HttpGet("faculties")]
    public Task<IActionResult> Faculties(CancellationToken ct)
        => Execute(() => workflow.FacultiesAsync(CurrentUserId(), ct));

    [HttpGet("directory-users")]
    public Task<IActionResult> SearchUsers([FromQuery] string query, [FromQuery] Guid? facultyId,
        CancellationToken ct)
        => Execute(() => workflow.SearchUsersAsync(CurrentUserId(), query, facultyId, ct));

    [HttpGet("{courseId:guid}")]
    public Task<IActionResult> Details(Guid courseId, CancellationToken ct)
        => Execute(() => workflow.DetailsAsync(CurrentUserId(), courseId, ct));

    [HttpGet("{courseId:guid}/certificate-eligibility")]
    public Task<IActionResult> CertificateEligibility(Guid courseId, CancellationToken ct)
        => Execute(() => certificateGate.StatusAsync(CurrentUserId(), courseId, ct));

    [HttpPost]
    public Task<IActionResult> Create([FromBody] CreateCourseInput input, CancellationToken ct)
        => Execute(() => workflow.CreateAsync(CurrentUserId(), input, ct));

    [HttpPost("{courseId:guid}/approve")]
    public Task<IActionResult> Approve(Guid courseId, [FromBody] ApproveCourseRequest request, CancellationToken ct)
        => Execute(() => workflow.ApproveAsync(CurrentUserId(), courseId, request.EvaluationDeadlineUtc,
            request.QualityOperated, request.InstructorUserId, ct));

    [HttpPost("{courseId:guid}/review")]
    public Task<IActionResult> ReviewProposal(Guid courseId, [FromBody] CourseProposalReviewRequest request,
        CancellationToken ct)
        => Execute(() => workflow.ReviewProposalAsync(CurrentUserId(), courseId,
            request.Decision, request.NoteAr, ct));

    [HttpPut("{courseId:guid}/resubmit")]
    public Task<IActionResult> ResubmitProposal(Guid courseId, [FromBody] CreateCourseInput input,
        CancellationToken ct)
        => Execute(() => workflow.ResubmitProposalAsync(CurrentUserId(), courseId, input, ct));

    [HttpPost("{courseId:guid}/enroll")]
    public Task<IActionResult> SelfEnroll(Guid courseId, CancellationToken ct)
        => Execute(() => workflow.SelfEnrollAsync(CurrentUserId(), courseId, ct));

    [HttpPut("{courseId:guid}/evaluation-deadline")]
    public Task<IActionResult> ExtendEvaluation(Guid courseId, [FromBody] EvaluationDeadlineRequest request,
        CancellationToken ct)
        => Execute(() => workflow.ExtendEvaluationAsync(CurrentUserId(), courseId, request.DeadlineUtc, ct));

    [HttpPost("{courseId:guid}/participants")]
    public Task<IActionResult> AddParticipant(Guid courseId, [FromBody] ParticipantRequest request, CancellationToken ct)
        => Execute(() => workflow.SelectParticipantAsync(CurrentUserId(), courseId, request.UserId, ct));

    [HttpPost("{courseId:guid}/lectures")]
    public Task<IActionResult> AddLecture(Guid courseId, [FromBody] NamedRecordRequest request, CancellationToken ct)
        => Execute(() => workflow.AddLectureAsync(CurrentUserId(), courseId, request.NameAr, request.NameEn,
            request.StartsAtUtc, ct));

    [HttpPost("lectures/{lectureId:guid}/activate-code")]
    public async Task<IActionResult> ActivateCode(Guid lectureId, CancellationToken ct)
    {
        try
        {
            var lecture = await workflow.ActivateCodeAsync(CurrentUserId(), lectureId, ct);
            return Ok(new { lecture.AttendanceCode, ExpiresAtUtc = lecture.CodeActivatedAtUtc?.AddMinutes(5) });
        }
        catch (Exception ex) when (IsBusinessError(ex)) { return BusinessError(ex); }
    }

    [HttpGet("lectures/{lectureId:guid}/code")]
    public async Task<IActionResult> Code(Guid lectureId, CancellationToken ct)
    {
        try
        {
            var lecture = await workflow.OperatorLectureAsync(CurrentUserId(), lectureId, ct);
            return Ok(new { lecture.AttendanceCode, ExpiresAtUtc = lecture.CodeActivatedAtUtc?.AddMinutes(5) });
        }
        catch (Exception ex) when (IsBusinessError(ex)) { return BusinessError(ex); }
    }

    [HttpPost("lectures/{lectureId:guid}/attendance-code")]
    public Task<IActionResult> EnterCode(Guid lectureId, [FromBody] AttendanceCodeRequest request, CancellationToken ct)
        => Execute(() => workflow.EnterCodeAsync(CurrentUserId(), lectureId, request.Code, ct));

    [HttpPut("lectures/{lectureId:guid}/attendance/{enrollmentId:guid}")]
    public Task<IActionResult> SetAttendance(Guid lectureId, Guid enrollmentId,
        [FromBody] ManualAttendanceRequest request, CancellationToken ct)
        => Execute(() => workflow.SetAttendanceAsync(CurrentUserId(), lectureId, enrollmentId, request.IsPresent, ct));

    [HttpPost("{courseId:guid}/end")]
    public Task<IActionResult> End(Guid courseId, CancellationToken ct)
        => Execute(() => workflow.EndAsync(CurrentUserId(), courseId, ct));

    [HttpPost("{courseId:guid}/exam")]
    public Task<IActionResult> CreateExam(Guid courseId, [FromBody] NamedRecordRequest request, CancellationToken ct)
        => Execute(() => workflow.CreateExamAsync(CurrentUserId(), courseId, request.NameAr, request.NameEn, ct));

    [HttpPut("{courseId:guid}/exam-requirement")]
    public Task<IActionResult> ChangeExamRequirement(Guid courseId,
        [FromBody] ExamRequirementRequest request, CancellationToken ct)
        => Execute(() => workflow.ChangeExamRequirementAsync(CurrentUserId(), courseId, request.Required, ct));

    [HttpPut("{courseId:guid}/exam/results/{enrollmentId:guid}")]
    public Task<IActionResult> ExamResult(Guid courseId, Guid enrollmentId,
        [FromBody] ExamResultRequest request, CancellationToken ct)
        => Execute(() => workflow.RecordExamResultAsync(CurrentUserId(), courseId, enrollmentId, request.Outcome, ct));

    private Guid CurrentUserId()
        => Guid.TryParse(User.FindFirstValue("sub"), out var id) ? id : throw new UnauthorizedAccessException();

    private async Task<IActionResult> Execute<T>(Func<Task<T>> action)
    {
        try { return Ok(await action()); }
        catch (Exception ex) when (IsBusinessError(ex)) { return BusinessError(ex); }
    }

    private static bool IsBusinessError(Exception ex)
        => ex is ArgumentException or InvalidOperationException or KeyNotFoundException or UnauthorizedAccessException;

    private IActionResult BusinessError(Exception ex) => ex switch
    {
        UnauthorizedAccessException => Forbid(),
        KeyNotFoundException => NotFound(new { message = ex.Message }),
        _ => BadRequest(new { message = ex.Message })
    };
}

public sealed record ApproveCourseRequest(DateTimeOffset EvaluationDeadlineUtc,
    bool QualityOperated, Guid? InstructorUserId);
public sealed record ParticipantRequest(Guid UserId);
public sealed record NamedRecordRequest(string NameAr, string? NameEn, DateTimeOffset? StartsAtUtc);
public sealed record AttendanceCodeRequest(string Code);
public sealed record ManualAttendanceRequest(bool IsPresent);
public sealed record ExamResultRequest(ExamOutcome Outcome);
public sealed record EvaluationDeadlineRequest(DateTimeOffset DeadlineUtc);
public sealed record ExamRequirementRequest(bool Required);
public sealed record CourseProposalReviewRequest(CourseStatus Decision, string NoteAr);
