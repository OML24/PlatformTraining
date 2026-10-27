using ZU.Training.Application.Security;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Training;

public sealed class HistoricalCourseService(
    IHistoricalCourseStore store,
    CertificateService certificateService,
    IPermissionAccess permissions)
{
    public async Task<HistoricalCourseResult> CreateHistoricalCourseAsync(
        Guid actorId, HistoricalCourseInput input, string portalBaseUrl, CancellationToken ct)
    {
        var canCreate = await permissions.IsAllowedAsync(actorId, PermissionKeys.ApproveCourses, ct) ||
                        await permissions.IsAllowedAsync(actorId, PermissionKeys.ManageAllCourses, ct);
        if (!canCreate) throw new UnauthorizedAccessException();

        if (string.IsNullOrWhiteSpace(input.TitleAr) || input.TitleAr.Length > 500 ||
            string.IsNullOrWhiteSpace(input.ObjectivesAr) || string.IsNullOrWhiteSpace(input.TargetAudienceAr) ||
            string.IsNullOrWhiteSpace(input.ActualInstructorNameAr) || input.PlannedHours <= 0)
            throw new ArgumentException("Required course details are missing or invalid.");

        if (input.LectureTitles is null || input.LectureTitles.Count == 0)
            throw new ArgumentException("Historical course must have at least one lecture.");

        var track = await store.TrackAsync(input.TrainingTrackId, ct)
            ?? throw new KeyNotFoundException("Training track not found.");

        var latestEval = await store.LatestEvaluationVersionAsync(ct);

        var course = new Course
        {
            TitleAr = input.TitleAr.Trim(),
            TitleEn = string.IsNullOrWhiteSpace(input.TitleEn) ? null : input.TitleEn.Trim(),
            TrainingTrackId = track.Id,
            ObjectivesAr = input.ObjectivesAr.Trim(),
            ObjectivesEn = string.IsNullOrWhiteSpace(input.ObjectivesEn) ? null : input.ObjectivesEn.Trim(),
            TargetAudienceAr = input.TargetAudienceAr.Trim(),
            TargetAudienceEn = string.IsNullOrWhiteSpace(input.TargetAudienceEn) ? null : input.TargetAudienceEn.Trim(),
            DeliveryMode = input.DeliveryMode,
            PlannedHours = input.PlannedHours,
            PlannedStartUtc = input.PlannedStartUtc,
            PlannedEndUtc = input.PlannedEndUtc,
            FacultyId = input.FacultyId,
            CreatedByUserId = actorId,
            ActualInstructorNameAr = input.ActualInstructorNameAr.Trim(),
            ActualInstructorNameEn = string.IsNullOrWhiteSpace(input.ActualInstructorNameEn) ? null : input.ActualInstructorNameEn.Trim(),
            QualityOperated = true,
            HasExam = input.HasExam,
            EnrollmentMode = EnrollmentMode.QualitySelection,
            Status = CourseStatus.Ended,
            EndedAtUtc = input.PlannedEndUtc,
            IsArchived = true,
            EvaluationFormVersionId = latestEval?.Id
        };
        store.AddCourse(course);
        await store.SaveAsync(ct);

        // Add Lectures
        var lectures = new List<Lecture>();
        for (var i = 0; i < input.LectureTitles.Count; i++)
        {
            var lecture = new Lecture
            {
                CourseId = course.Id,
                TitleAr = input.LectureTitles[i].Trim(),
                SequenceNumber = i + 1,
                StartsAtUtc = input.PlannedStartUtc.AddHours(i * 2),
                AttendanceCode = Random.Shared.Next(1000, 9999).ToString()
            };
            lectures.Add(lecture);
            store.AddLecture(lecture);
        }
        await store.SaveAsync(ct);

        // Add Exam if applicable
        Exam? exam = null;
        if (input.HasExam)
        {
            exam = new Exam
            {
                CourseId = course.Id,
                NameAr = "الامتحان النهائي التوثيقي",
                NameEn = "Final Documentation Exam"
            };
            store.AddExam(exam);
            await store.SaveAsync(ct);
        }

        var certificatesIssued = 0;

        // Process Participants
        if (input.Participants is not null)
        {
            foreach (var p in input.Participants)
            {
                var user = await store.UserAsync(p.UserId, ct);
                if (user is null || !user.IsActive) continue;

                var enrollment = new CourseEnrollment
                {
                    CourseId = course.Id,
                    UserId = user.Id,
                    EnrolledAtUtc = input.PlannedStartUtc
                };
                store.AddEnrollment(enrollment);
                await store.SaveAsync(ct);

                // Attendance
                for (var i = 0; i < lectures.Count; i++)
                {
                    var isPresent = p.AttendedLectureIndices.Contains(i);
                    store.AddAttendance(new AttendanceRecord
                    {
                        LectureId = lectures[i].Id,
                        EnrollmentId = enrollment.Id,
                        IsPresent = isPresent,
                        SetByInstructor = true,
                        LastChangedByUserId = actorId
                    });
                }

                // Exam Result
                if (exam is not null && p.ExamOutcome is ExamOutcome.Passed or ExamOutcome.Failed)
                {
                    store.AddExamResult(new ExamResult
                    {
                        ExamId = exam.Id,
                        EnrollmentId = enrollment.Id,
                        Outcome = p.ExamOutcome,
                        RecordedByUserId = actorId
                    });
                }

                // Evaluation
                if (p.EvaluationCompleted && latestEval is not null)
                {
                    var evalResponse = new EvaluationResponse
                    {
                        FormVersionId = latestEval.Id,
                        EnrollmentId = enrollment.Id
                    };
                    store.AddEvaluationResponse(evalResponse);
                    var questions = await store.EvaluationQuestionsAsync(latestEval.Id, ct);
                    foreach (var q in questions)
                    {
                        var opt = await store.FirstEvaluationOptionAsync(q.Id, ct);
                        if (opt is not null)
                        {
                            store.AddEvaluationAnswer(new EvaluationAnswer
                            {
                                ResponseId = evalResponse.Id,
                                QuestionId = q.Id,
                                OptionId = opt.Id
                            });
                        }
                    }
                }

                // Impact
                if (p.ImpactApproved)
                {
                    var hashBytes = new byte[32];
                    Random.Shared.NextBytes(hashBytes);
                    store.AddImpactSubmission(new ImpactSubmission
                    {
                        EnrollmentId = enrollment.Id,
                        Version = 1,
                        OriginalName = "خطة_الأثر_التدريبي_المعتمدة.pdf",
                        StorageKey = $"{Guid.NewGuid():N}.pdf",
                        Sha256 = Convert.ToHexStringLower(hashBytes),
                        Status = ImpactStatus.Approved,
                        ReviewedByUserId = actorId,
                        ReviewedAtUtc = input.PlannedEndUtc
                    });
                }

                await store.SaveAsync(ct);

                // Issue certificate if conditions met
                var cert = await certificateService.IssueOrReevaluateCertificateAsync(
                    course.Id, enrollment.Id, portalBaseUrl, ct);

                if (cert is not null && cert.Status == CertificateStatus.Valid)
                    certificatesIssued++;
            }
        }

        return new HistoricalCourseResult(course.Id, input.Participants?.Count ?? 0, certificatesIssued);
    }
}
