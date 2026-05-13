using Coursna.Core.Domain.Entities;
using Coursna.Core.Domain.Enums;
using Coursna.Core.Domain.RepositoryInterface;
using Coursna.Core.Dtos;
using Coursna.Core.Exceptions;
using Coursna.Core.ServiceContracts;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Service
{
    public class AttemptService : IAttemptService
    {

        private readonly IAttemptRepository _attemptRepo;
        private readonly IRepository<QuizAttempt> _repository ;
        private readonly IRepository<Quiz> _quizRepo;
        private readonly IQuizRepository _quizRepoCustom;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly IEnrollmentRepository _enrollmentRepo;
        public AttemptService(IAttemptRepository attemptRepo, IRepository<Quiz> quizRepo, IRepository<QuizAttempt> repository, IEnrollmentRepository enrollmentRepo, IQuizRepository quizRepoCustom , IBackgroundJobClient backgroundJobClient)
        {
            _attemptRepo = attemptRepo;
            _quizRepo = quizRepo;
            _repository = repository;
            _quizRepoCustom = quizRepoCustom;
            _enrollmentRepo = enrollmentRepo;
            _backgroundJobs = backgroundJobClient;


        }

        public async Task SaveResponseAsync(int attemptId, SaveResponseRequest request, string studentId)
        {
            

            var attempt = await _attemptRepo.GetByIdWithFullQuizDataAsync(attemptId,studentId)
                ?? throw new KeyNotFoundException("Attempt not found.");

            if (studentId != attempt.StudentId)
            {
                throw new UnauthorizedAccessException("student not applicable");

            }
            if (attempt.Status != AttemptStatus.InProgress)
                throw new UnauthorizedAccessException("Attempt is no longer active.");

            if (attempt.IsTimeExpired())
            {
                await SubmitAttemptAsync(attemptId, studentId); 
                throw new UnauthorizedAccessException("Time limit expired. Quiz has been auto-submitted.");
            }

            var question = attempt.Quiz.Questions.FirstOrDefault(q => q.Id == request.QuestionId)
                ?? throw new ArgumentException("Invalid Question ID for this quiz.");

            var option = question.Options.FirstOrDefault(o => o.Id == request.OptionId)
                ?? throw new ArgumentException("Invalid Option ID for this question.");

            var existingResponse = attempt.Responses.FirstOrDefault(r => r.QuestionId == request.QuestionId);
            if (existingResponse != null)
            {
                existingResponse.OptionId = request.OptionId;
            }
            else
            {
                attempt.Responses.Add(new StudentResponse
                {
                    QuizAttemptId = attemptId,
                    QuestionId = request.QuestionId,
                    OptionId = request.OptionId
                });
            }

            await _repository.SaveChangesAsync();
        }


        


        public async Task<int> StartAttemptAsync(int quizId, string studentId)
        {
            var quiz = await _quizRepo.GetByIdAsync(quizId)
                ?? throw new KeyNotFoundException("Quiz not found.");
            var enrollment = await _enrollmentRepo
            .GetActiveEnrollmentAsync(studentId, quiz.CourseId);

            if (enrollment == null)
                throw new NotFoundException("Access denied");

            if (!quiz.IsPublished)
                throw new InvalidOperationException("Cannot start an unpublished quiz.");

            
            var activeAttempt = await _attemptRepo.GetActiveAttemptAsync(quizId, studentId);
            if (activeAttempt != null)
                throw new InvalidOperationException("You already have an active attempt for this quiz.");

            if (quiz.MaxAttempts > 0)
            {
                var previousAttemptsCount = await _attemptRepo.GetAttemptCountAsync(quizId, studentId);

                if (previousAttemptsCount >= quiz.MaxAttempts)
                {

                    throw new UnauthorizedAccessException(
                        $"You have reached the maximum number of attempts ({quiz.MaxAttempts}) for this quiz.");
                }
            }
                var attempt = new QuizAttempt
                {
                    QuizId = quizId,
                    StudentId = studentId,
                    Status = AttemptStatus.InProgress,
                    StartedAt = DateTime.UtcNow
                };

                await _repository.AddAsync(attempt);
                await _repository.SaveChangesAsync();
                _backgroundJobs.Schedule<IQuizTimeoutJob>(
                 job => job.AutoSubmitAttemptAsync(attempt.Id,studentId),
                 TimeSpan.FromMinutes(quiz.TimeLimitInMinutes)
                 );
                return attempt.Id;


            }
        public async Task<QuizWithQuestionsDto> GetAttemptQuestionsAsync(int attemptId, string studentId)
        {
            // 1. Fetch the attempt and include the Quiz + Questions + Options
            var attempt = await _attemptRepo.GetByIdWithFullQuizDataAsync(attemptId,studentId);


            // 2. Security Check
            if (attempt == null)
                throw new KeyNotFoundException("Attempt not found.");

            if (attempt.Status != AttemptStatus.InProgress || attempt.IsTimeExpired())
                throw new UnauthorizedAccessException("This attempt is closed.");

            return new QuizWithQuestionsDto
            {
                Id = attempt.Quiz.Id,
                Title = attempt.Quiz.Title,
                CourseId = attempt.Quiz.CourseId,
                IsPublished = attempt.Quiz.IsPublished,
                TimeLimitInMinutes = attempt.Quiz.TimeLimitInMinutes,
                Questions = attempt.Quiz.Questions.Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.Title,
                    Options = q.Options.Select(o => new OptionDto
                    {
                        Id = o.Id,
                        Text = o.Text
                    }).ToList()
                }).ToList()
            };
        }

        public async Task<AttemptResultResponse> SubmitAttemptAsync(int attemptId, string studentId)
        {
            var attempt = await _attemptRepo.GetByIdWithFullQuizDataAsync(attemptId, studentId)
               ?? throw new KeyNotFoundException("Attempt not found.");

            var maxScore = attempt.Quiz.Questions.Sum(q => q.Points);

            // If already completed, just return the existing result instead of throwing
            // This handles cases where manual submit and auto-submit happen nearly simultaneously
            if (attempt.Status == AttemptStatus.Completed)
            {
                return new AttemptResultResponse
                {
                    AttemptId = attempt.Id,
                    QuizId = attempt.QuizId,
                    QuizTitle = attempt.Quiz.Title,
                    TotalScore = maxScore > 0 ? (attempt.CurrentScore / maxScore) * 100 : 0,
                    CompletedAt = attempt.CompletedAt ?? DateTime.UtcNow
                };
            }

            if (attempt.Status != AttemptStatus.InProgress)
                throw new InvalidOperationException("Attempt cannot be submitted in its current state.");

            var enrollment = await _enrollmentRepo
                .GetActiveEnrollmentAsync(studentId, attempt.Quiz.CourseId);

            if (enrollment == null)
                throw new NotFoundException("Access denied");

            decimal totalScore = 0;
            // Ensure we only process the latest response per question to prevent any double-counting
            var distinctResponses = attempt.Responses
                .GroupBy(r => r.QuestionId)
                .Select(g => g.Last());

            foreach (var response in distinctResponses)
            {
                var question = attempt.Quiz.Questions.First(q => q.Id == response.QuestionId);
                var selectedOption = question.Options.FirstOrDefault(o => o.Id == response.OptionId);

                if (selectedOption != null && selectedOption.IsCorrect)
                {
                    totalScore += question.Points;
                }
            }

            attempt.CurrentScore = totalScore;
            attempt.Status = AttemptStatus.Completed;
            attempt.CompletedAt = DateTime.UtcNow;

            await _repository.SaveChangesAsync();

            return new AttemptResultResponse
            {
                AttemptId = attempt.Id,
                QuizId = attempt.QuizId,
                QuizTitle = attempt.Quiz.Title,
                TotalScore = maxScore > 0 ? (attempt.CurrentScore / maxScore) * 100 : 0,
                CompletedAt = attempt.CompletedAt.Value
            };
        }
        public async Task<AttemptResultResponse> GetAttemptResultAsync(int attemptId, string studentId)
        {
            var attempt = await _attemptRepo.GetByIdWithFullQuizDataAsync(attemptId, studentId)
                 ?? throw new KeyNotFoundException("Attempt not found.");

            if (attempt == null) throw new KeyNotFoundException("Attempt not found.");
            var enrollment = await _enrollmentRepo
                .GetActiveEnrollmentAsync(studentId, attempt.Quiz.CourseId);
            if (enrollment == null)
                throw new NotFoundException("Access denied");   
            // If the timer is up but the job hasn't run yet, we still treat it as closed
            if (attempt.Status != AttemptStatus.Completed && attempt.CompletedAt > DateTime.UtcNow)
            {
                throw new InvalidOperationException("Quiz is still in progress.");
            }

            var maxScore = attempt.Quiz.Questions.Sum(q => q.Points);

            return new AttemptResultResponse
            {
                AttemptId = attempt.Id,
                QuizId = attempt.QuizId,
                QuizTitle = attempt.Quiz.Title,
                TotalScore = maxScore > 0 ? (attempt.CurrentScore / maxScore) * 100 : 0,
                CompletedAt = attempt.CompletedAt.Value
            };
        }

        public async Task<ActiveAttemptDto> GetActiveAttemptAsync(int quizId, string studentId)
        {
            var activeAttempt = await _attemptRepo.GetActiveAttemptAsync(quizId, studentId);

            if (activeAttempt == null) return new ActiveAttemptDto { AttemptId = null };

            if (activeAttempt.IsTimeExpired())
            {
                try
                {
                    await SubmitAttemptAsync(activeAttempt.Id, studentId);
                }
                catch { }
                return new ActiveAttemptDto { AttemptId = null };
            }

            var expiresAt = activeAttempt.StartedAt.AddMinutes(activeAttempt.Quiz.TimeLimitInMinutes);
            var remaining = expiresAt - DateTime.UtcNow;

            return new ActiveAttemptDto
            {
                AttemptId = activeAttempt.Id,
                RemainingHours = Math.Max(0, remaining.Hours + (remaining.Days * 24)),
                RemainingMinutes = Math.Max(0, remaining.Minutes)
            };
        }

        public async Task<List<AttemptResultResponse>> GetStudentAttemptsByCourseIdAsync(string studentId, int courseId)
        {
            var enrollment = await _enrollmentRepo
                .GetActiveEnrollmentAsync(studentId, courseId);

            if (enrollment == null)
                throw new NotFoundException("Access denied");

            var attempts = await _attemptRepo.GetStudentAttemptsByCourseIdAsync(studentId, courseId);

            return attempts.Select(a =>
            {
                var maxScore = a.Quiz.Questions.Sum(q => q.Points);
                return new AttemptResultResponse
                {
                    AttemptId = a.Id,
                    QuizId = a.QuizId,
                    QuizTitle = a.Quiz.Title,
                    TotalScore = maxScore > 0 ? (a.CurrentScore / maxScore) * 100 : 0,
                    CompletedAt = a.CompletedAt ?? DateTime.UtcNow
                };
            }).ToList();
        }
    }
}

