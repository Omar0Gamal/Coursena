using Coursna.Core.Domain.Entities;
using Coursna.Core.Domain.RepositoryInterface;
using Coursna.Core.Dtos;
using Coursna.Core.Exceptions;
using Coursna.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Service
{
    public class QuizService : IQuizService
    {
        private IRepository<Quiz> _Repository;
        private readonly IQuizRepository _quizRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly IAttemptRepository _attemptRepository;

        public QuizService(IRepository<Quiz> repository, IQuizRepository quizrepo, IRepository<Course> courseRepository, IEnrollmentRepository enrollmentRepo, IAttemptRepository attemptRepository)
        {
            _Repository = repository;
            _quizRepository = quizrepo;
            _courseRepository = courseRepository;
            _enrollmentRepo = enrollmentRepo;
            _attemptRepository = attemptRepository;
        }

        public async Task<QuizResponseDto> CreateQuizAsync(CreateQuizDto dto,string teacherId)
        {
            var course=await _courseRepository.GetByIdAsync(dto.CourseId);
            if (course == null)
            {
                throw new Exception("course not found");
            }
            if(course.TeacherId != teacherId)
            {
                throw new Exception("you are not the teacher of this course");
            }
            Quiz quiz = new Quiz
            {
                Title = dto.Title,
                CourseId = dto.CourseId,
              
                IsPublished = false,
                MaxAttempts = dto.MaxAttempts,
                TimeLimitInMinutes = dto.TimeLimitInMinutes
            };
            await _Repository.AddAsync(quiz);
            await _Repository.SaveChangesAsync();
            QuizResponseDto response = new QuizResponseDto
            {
                Id = quiz.Id,
                Title = quiz.Title,
                CourseId = quiz.CourseId,
                IsPublished = quiz.IsPublished,
                MaxAttempts = quiz.MaxAttempts,
                TimeLimitInMinutes = quiz.TimeLimitInMinutes
            };
            return response;

        }

 
public async Task<QuizResponseDto?> GetQuizByIdAsync(int quizId)
        {
            var quiz = await _Repository.GetByIdAsync(quizId);
            if (quiz == null)
            {
                throw new Exception("quiz not found");
            }
            var response = new QuizResponseDto()
            {
                Title = quiz.Title,
                CourseId = quiz.CourseId,
                IsPublished = quiz.IsPublished,
                MaxAttempts = quiz.MaxAttempts,
                TimeLimitInMinutes = quiz.TimeLimitInMinutes
            };
            return response;
        }

        public async Task<QuizWithQuestionsDto?> GetStudentQuizWithQuestionsByIdAsync(int quizId,string studentId)
        {
            var quiz = await _quizRepository.GetStudentQuizWithQuestionsAsync(quizId);
            if (quiz == null)
            {
                throw new Exception("quiz not found");
            }
            var enrollment = await _enrollmentRepo
            .GetActiveEnrollmentAsync(studentId, quiz.CourseId);

            if (enrollment == null)
                throw new NotFoundException("Access denied");
            var response = new QuizWithQuestionsDto()
            {
                Id = quiz.Id,
                Title = quiz.Title,
                CourseId = quiz.CourseId,
                IsPublished = quiz.IsPublished,
                MaxAttempts = quiz.MaxAttempts,
                TimeLimitInMinutes = quiz.TimeLimitInMinutes,
                Questions = quiz.Questions.Select(q => new QuestionDto
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
            return response;
        }

        


        public async Task<QuizResponseDto> UpdateQuizAsync(int quizId, CreateQuizDto dto, string teacherId)
        {
            var quiz = await _quizRepository.GetQuizForTeacherAsync(quizId, teacherId   );

            if (quiz == null)
            {
                throw new Exception("quiz not found");
            }
            if (quiz.IsPublished == true)
            {
                throw new Exception("cannot update a published quiz");
            }

            quiz.Title = dto.Title;
            quiz.IsPublished = dto.IsPublished;
            quiz.MaxAttempts = dto.MaxAttempts;
            quiz.TimeLimitInMinutes = dto.TimeLimitInMinutes;
            await _Repository.UpdateAsync(quiz);
            await _Repository.SaveChangesAsync();
            var response = new QuizResponseDto()
            {
                Title = quiz.Title,
                CourseId = quiz.CourseId,
                IsPublished = quiz.IsPublished,
                MaxAttempts = quiz.MaxAttempts,
                TimeLimitInMinutes = quiz.TimeLimitInMinutes
            };
            return response;
        }

        public async Task<bool> DeleteQuizAsync(int quizId, string teacherId)
        {
            var quiz = await _quizRepository.GetQuizForTeacherAsync(quizId, teacherId);

            if (quiz == null)
                return false;

            _Repository.DeleteAsync(quiz);
            await _Repository.SaveChangesAsync();

            return true;
        }

        public async Task PublishQuizAsync(int quizId, string teacherId)
        {

            var quiz = await _quizRepository.GetQuizWithQuestionsAsync(quizId,teacherId);

            if (quiz == null)
            {
                throw new Exception("quiz not found");
            }
            if (!quiz.Questions.Any())
            {
                throw new Exception("cannot publish a quiz without questions");
            }
            quiz.IsPublished = true;
            await _Repository.UpdateAsync(quiz);
            await _Repository.SaveChangesAsync();
        }
        public async Task<int> AddQuestionAsync(int quizId, CreateQuestionDto dto, string teacherId)
        {
            var quiz = await _quizRepository.GetQuizWithQuestionsAsync(quizId, teacherId);
            if (quiz == null)
            {
                throw new KeyNotFoundException("Quiz not found.");
            }
            if (quiz.IsPublished == true)
            {
                throw new InvalidOperationException("Cannot add questions to a published quiz.");
            }
            if (dto.Points <= 0)
                throw new ArgumentException("Question points must be greater than zero.");
            var question = new Question
            {
                Title = dto.Title,
                Points = dto.Points,
                QuizId = quizId,
                Options = dto.Options.Select(o => new Option
                {
                    Text = o.Text,
                    IsCorrect = o.IsCorrect
                }).ToList()

            };
            quiz.Questions.Add(question);
            await _Repository.SaveChangesAsync();
            return question.Id;


        }

        public async Task<List<QuizResponseDto>> GetQuizzesByCourseIdAsync(int courseId, string teacherId)
        {
            var quizzes = await _quizRepository.GetQuizzesByCourseIdAsync(courseId, teacherId);

            return quizzes.Select(q => new QuizResponseDto { 
               Id= q.Id,
               CourseId =q.CourseId,
                Title=q.Title,
                TimeLimitInMinutes=q.TimeLimitInMinutes,
                IsPublished=q.IsPublished,
                CreatedAt=q.CreatedAt,
                MaxAttempts=q.MaxAttempts }
              
            ).ToList();
        }

        public async Task<List<PublishedQuizDto>> GetPublishedQuizzesByCourseIdAsyc(int courseId, string studentId)
        {
            var quizzes = await _quizRepository.GetPublishedByCourseIdAsync(courseId);

            if (quizzes == null)
                throw new Exception("quizzes is NULL");

            var enrollment = await _enrollmentRepo
                .GetActiveEnrollmentAsync(studentId, courseId);

            if (enrollment == null)
                throw new NotFoundException("Access denied");

            foreach (var q in quizzes)
            {
                if (q == null)
                    throw new Exception("quiz item is NULL");
            }

            return quizzes.Select(q => new PublishedQuizDto
            {
                Id = q.Id,
                CourseId = q.CourseId,
                Title = q.Title,
                TimeLimitInMinutes = q.TimeLimitInMinutes
            }).ToList();
        }

        public async Task<QuizWithQuestionWithAnswersDto> GetTeacherQuizWithQuestionsByIdAsync(int quizId, string teacherId)
        {
            var quiz = await _quizRepository.GetQuizWithQuestionsAsync(quizId, teacherId);
            if (quiz == null)
            {
                throw new Exception("quiz not found");
            }

            var response = new QuizWithQuestionWithAnswersDto()
            {
                Id = quiz.Id,
                Title = quiz.Title,
                CourseId = quiz.CourseId,
                IsPublished = quiz.IsPublished,
                MaxAttempts = quiz.MaxAttempts,
                TimeLimitInMinutes = quiz.TimeLimitInMinutes,
                Questions = quiz.Questions.Select(q => new QuestionWithAnswerDto
                {
                    Id = q.Id,
                    Text = q.Title,
                    Point = q.Points,
                    Options = q.Options.Select(o => new OptionWithCorretDto
                    {
                        Id = o.Id,
                        Text = o.Text,
                        IsCorrect = o.IsCorrect
                    }).ToList()
                }).ToList()
            };
            return response;
        }

        public async Task<QuizWithAnswersDto> GetTeacherQuizWithAnswersByIdAsync(int quizId, string teacherId)
        {
            var quiz = await _quizRepository.GetQuizWithQuestionsAsync(quizId, teacherId);
            if (quiz == null)
            {
                throw new Exception("quiz not found");
            }

            var response = new QuizWithAnswersDto()
            {
                Id = quiz.Id,
                Title = quiz.Title,
                CourseId = quiz.CourseId,
                IsPublished = quiz.IsPublished,
                MaxAttempts = quiz.MaxAttempts,
                TimeLimitInMinutes = quiz.TimeLimitInMinutes,
                Questions = quiz.Questions.Select(q => new Questions
                {
                    Id = q.Id,
                    Text = q.Title,
                    Options = q.Options.Select(o => new OptionWithAnswer
                    {
                        Id = o.Id,
                        Text = o.Text,
                        IsCorrect = o.IsCorrect
                    }).ToList()
                }).ToList()
            };
            return response;
        }

        public async Task<List<QuizResultDto>> GetQuizResultsAsync(int quizId, string teacherId)
        {
            var quiz = await _quizRepository.GetQuizForTeacherAsync(quizId, teacherId);
            if (quiz == null)
            {
                throw new KeyNotFoundException("Quiz not found or you don't have access.");
            }

            var attempts = await _attemptRepository.GetQuizResultsAsync(quizId);

            return attempts.Select(a =>
            {
                var totalCount = a.Quiz.Questions.Count;
                var maxScore = a.Quiz.Questions.Sum(q => q.Points);
                var scorePercentage = maxScore > 0 ? (a.CurrentScore / maxScore) * 100 : 0;
                var correctCount = a.Responses.Count(r =>
                    a.Quiz.Questions.Any(q => q.Id == r.QuestionId &&
                    q.Options.Any(o => o.Id == r.OptionId && o.IsCorrect)));

                return new QuizResultDto
                {
                    StudentName = a.Student?.FullName ?? "Unknown",
                    Score = scorePercentage,
                    MaxScore = maxScore,
                    CorrectCount = correctCount,
                    TotalCount = totalCount,
                    CompletedAt = a.CompletedAt
                };
            }).ToList();
        }
    }
}

