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
        //private readonly IQuizRepository _quizRepo;
        private readonly IRepository<QuizAttempt> _repository ;
        private readonly IRepository<Quiz> _quizRepo;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly IEnrollmentRepository _enrollmentRepo;
        public AttemptService(IAttemptRepository attemptRepo, IRepository<Quiz> quizRepo, IRepository<QuizAttempt> repository, IEnrollmentRepository enrollmentRepo, IBackgroundJobClient backgroundJobs)
        {
            _attemptRepo = attemptRepo;
            _quizRepo = quizRepo;
            _repository = repository;
            _enrollmentRepo = enrollmentRepo;
            _backgroundJobs = backgroundJobs;

        }

        public async Task SaveResponseAsync(int attemptId, SaveResponseRequest request, string studentId)
        {
            

            var attempt = await _attemptRepo.GetByIdWithFullQuizDataAsync(attemptId)
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


        


        public async Task<int> StartAttemptAsync(int quizId, StartAttemptRequest request,string studentId)
        {
            var quiz = await _quizRepo.GetByIdAsync(quizId)
                ?? throw new KeyNotFoundException("Quiz not found.");
            var enrollment = await _enrollmentRepo
            .GetActiveEnrollmentAsync(studentId, quiz.CourseId);

            if (enrollment == null)
                throw new NotFoundException("Access denied");

            if (!quiz.IsPublished)
                throw new InvalidOperationException("Cannot start an unpublished quiz.");

            
            var activeAttempt = await _attemptRepo.GetActiveAttemptAsync(quizId, request.StudentId);
            if (activeAttempt != null)
                throw new InvalidOperationException("You already have an active attempt for this quiz.");

            if (quiz.MaxAttempts > 0)
            {
                var previousAttemptsCount = await _attemptRepo.GetAttemptCountAsync(quizId, request.StudentId);

                if (previousAttemptsCount >= quiz.MaxAttempts)
                {

                    throw new UnauthorizedAccessException(
                        $"You have reached the maximum number of attempts ({quiz.MaxAttempts}) for this quiz.");
                }
            }
                var attempt = new QuizAttempt
                {
                    QuizId = quizId,
                    StudentId = request.StudentId,
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
        

        public async Task<AttemptResultResponse> SubmitAttemptAsync(int attemptId,string studentId)
        {

            var attempt = await _attemptRepo.GetByIdWithFullQuizDataAsync(attemptId)
               ?? throw new KeyNotFoundException("Attempt not found.");
            if (studentId != attempt.StudentId)
            {
                throw new UnauthorizedAccessException("student not applicable");

            }
            var enrollment = await _enrollmentRepo
            .GetActiveEnrollmentAsync(studentId, attempt.Quiz.CourseId);

            if (enrollment == null)
                throw new NotFoundException("Access denied");

            if (attempt.Status != AttemptStatus.InProgress)
                throw new InvalidOperationException("Attempt has already been submitted.");

            decimal totalScore = 0;

            foreach (var response in attempt.Responses)
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
                TotalScore = attempt.CurrentScore,
                CompletedAt = attempt.CompletedAt.Value
            };
        }
    }
    }

