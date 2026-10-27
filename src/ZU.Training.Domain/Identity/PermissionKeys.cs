namespace ZU.Training.Domain.Identity;

public static class PermissionKeys
{
    public const string ManageHints = "admin.hints.manage";
    public const string ManagePasswordPolicy = "admin.password_policy.manage";
    public const string ManageCertificateTemplates = "admin.certificate_templates.manage";
    public const string ManageUsers = "admin.users.manage";
    public const string ManagePermissions = "admin.permissions.manage";
    public const string ViewCourses = "courses.view";
    public const string CreateCourses = "courses.create";
    public const string ApproveCourses = "courses.approve";
    public const string SelfEnroll = "courses.self_enroll";
    public const string SelectParticipants = "courses.participants.select";
    public const string ManageLectures = "courses.lectures.manage";
    public const string EnterAttendanceCode = "courses.attendance.enter_code";
    public const string CorrectAttendance = "courses.attendance.correct";
    public const string EndCourse = "courses.end";
    public const string RecordExam = "courses.exam.record";
    public const string SearchUsers = "directory.users.search";
    public const string SyncUniversity = "admin.university_sync.run";
    public const string ViewTrainingNeeds = "training_needs.view";
    public const string SubmitTrainingNeeds = "training_needs.submit";
    public const string ReviewTrainingNeeds = "training_needs.review";
    public const string ViewCancellations = "course_cancellations.view";
    public const string RequestCancellation = "course_cancellations.request";
    public const string ReviewCancellation = "course_cancellations.review";
    public const string ViewOfficialDocuments = "official_documents.view";
    public const string SubmitImpact = "impact.submit";
    public const string ReviewImpact = "impact.review";
    public const string ManageEvaluation = "evaluation.manage";
    public const string SubmitEvaluation = "evaluation.submit";
    public const string ViewEvaluationStatistics = "evaluation.statistics.view";
    public const string ChangeExamRequirement = "courses.exam_requirement.change";
    public const string ManageAllCourses = "admin.courses.manage";

    public static IReadOnlyList<string> All { get; } =
    [
        ManageHints, ManagePasswordPolicy, ManageCertificateTemplates, ManageUsers, ManagePermissions, SyncUniversity,
        ViewCourses, CreateCourses, ApproveCourses, SelfEnroll, SelectParticipants,
        ManageLectures, EnterAttendanceCode, CorrectAttendance, EndCourse, RecordExam, SearchUsers,
        ViewTrainingNeeds, SubmitTrainingNeeds, ReviewTrainingNeeds, ViewCancellations,
        RequestCancellation, ReviewCancellation, ViewOfficialDocuments, SubmitImpact, ReviewImpact,
        ManageEvaluation, SubmitEvaluation, ViewEvaluationStatistics, ChangeExamRequirement,
        ManageAllCourses
    ];
}
