using Coursna.Core.Domain.Entities;
using Coursna.Core.Domain.IdentityEntities;
using Coursna.Core.Domain.RepositoryInterface;
using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Service
{
    public class QuizService : IQuizService
    {
        private IRepository<Quiz> _Repository;
        private readonly IQuizRepository _quizRepository;

        public QuizService(IRepository<Quiz> repository, IQuizRepository quizrepo)
        {
            _Repository = repository;
            _quizRepository = quizrepo;

        }

        public Task<QuizResponseDto> CreateQuizAsync(CreateQuizDto dto, int courseId)
        {
            Quiz quiz = new Quiz
            {
                Title = dto.Title,
                CourseId = courseId,
                Description = dto.Description,
                IsPublished = false,
                MaxAttempts = dto.MaxAttempts,
                TimeLimitInMinutes = dto.TimeLimitInMinutes
            };
            _Repository.AddAsync(quiz);
            _Repository.SaveChangesAsync();
            QuizResponseDto response = new QuizResponseDto
            {
                Title = quiz.Title,
                CourseId = quiz.CourseId,
                Description = quiz.Description,
                IsPublished = quiz.IsPublished,
                MaxAttempts = quiz.MaxAttempts,
                TimeLimitInMinutes = quiz.TimeLimitInMinutes
            };
            return Task.FromResult(response);

        }

        public async Task<List<QuizResponseDto>> GetQuizzesByCourseIdAsync(int courseId)
        {
            var course = await _Repository.GetByIdAsync(courseId);
            if (course == null)
                return null;
            var quizzes = await _quizRepository.GetQuizzesByCourseIdAsync(courseId);
            List<QuizResponseDto> response = quizzes.Select(q => new QuizResponseDto
            {
                Title = q.Title,
                CourseId = q.CourseId,
                Description = q.Description,
                IsPublished = q.IsPublished,
                MaxAttempts = q.MaxAttempts,
                TimeLimitInMinutes = q.TimeLimitInMinutes
            }).ToList();
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
                Description = quiz.Description,
                IsPublished = quiz.IsPublished,
                MaxAttempts = quiz.MaxAttempts,
                TimeLimitInMinutes = quiz.TimeLimitInMinutes
            };
            return response;
        }

        public async Task<QuizWithQuestionsDto?> GetQuizWithQuestionsByIdAsync(int quizId)
        {
            var quiz = await _quizRepository.GetQuizWithQuestionsAsync(quizId);
            if (quiz == null)
            {
                throw new Exception("quiz not found");
            }
            var response = new QuizWithQuestionsDto()
            {
                Id = quiz.Id,
                Title = quiz.Title,
                CourseId = quiz.CourseId,
                Description = quiz.Description,
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


        public async Task<QuizResponseDto> UpdateQuizAsync(int quizId, CreateQuizDto dto)
        {
            var quiz = await _Repository.GetByIdAsync(quizId);
            if (quiz == null)
            {
                throw new Exception("quiz not found");
            }
            if (quiz.IsPublished == true)
            {
                throw new Exception("cannot update a published quiz");
            }

            quiz.Title = dto.Title;
            quiz.Description = dto.Description;
            quiz.IsPublished = dto.IsPublished;
            quiz.MaxAttempts = dto.MaxAttempts;
            quiz.TimeLimitInMinutes = dto.TimeLimitInMinutes;
            await _Repository.UpdateAsync(quiz);
            await _Repository.SaveChangesAsync();
            var response = new QuizResponseDto()
            {
                Title = quiz.Title,
                CourseId = quiz.CourseId,
                Description = quiz.Description,
                IsPublished = quiz.IsPublished,
                MaxAttempts = quiz.MaxAttempts,
                TimeLimitInMinutes = quiz.TimeLimitInMinutes
            };
            return response;
        }

        public async Task<bool> DeleteQuizAsync(int quizId)
        {
            var quiz = await _Repository.GetByIdAsync(quizId);

            if (quiz == null)
                return false;


            _Repository.DeleteAsync(quiz);
            await _Repository.SaveChangesAsync();

            return true;
        }

        public async Task PublishQuizAsync(int quizId)
        {
            var quiz = await _quizRepository.GetQuizWithQuestionsAsync(quizId);
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
        public async Task<int> AddQuestionAsync(int quizId, CreateQuestionDto dto)
        {
            var quiz= await _Repository.GetByIdAsync(quizId);
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

        public Task AddQuestionAsync(int quizId, QuestionDto question)
        {
            throw new NotImplementedException();
        }
    }
}
