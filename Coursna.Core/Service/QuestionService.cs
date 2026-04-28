using Coursna.Core.Domain.Entities;
using Coursna.Core.Domain.RepositoryInterface;
using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Service
{
    public class QuestionService: IQuestionService
    {
        private IRepository<Question> _Repository;
        private IRepository<Option> _opRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IOptionRepository _optionRepository;

        public QuestionService(IRepository<Question> repository, IRepository<Option> oprepository, IQuestionRepository questionrepo, IOptionRepository optionRepository)
        {
            _Repository = repository;
            _questionRepository = questionrepo;
            _optionRepository = optionRepository;
            _opRepository = oprepository;


        }
    public async Task UpdateQuestionAsync(int questionId, CreateQuestionDto dto)
        {
            var question = await _questionRepository.GetByIdWithQuizAsync(questionId)
                ?? throw new KeyNotFoundException("Question not found.");

            if (question.Quiz.IsPublished)
                throw new InvalidOperationException("Cannot modify a question in a published quiz.");

            if (dto.Points <= 0)
                throw new ArgumentException("Points must be greater than zero.");

            question.Title = dto.Title;
            question.Points = dto.Points;

            await _Repository.SaveChangesAsync();
        }


        public async Task DeleteQuestionAsync(int questionId)
        {
            var question = await _questionRepository.GetByIdWithQuizAsync(questionId)
                ?? throw new KeyNotFoundException("Question not found.");
            if (question.Quiz.IsPublished)
                throw new InvalidOperationException("Cannot delete a question from a published quiz.");
             _Repository.DeleteAsync(question);
            await _Repository.SaveChangesAsync();

        }

        public async Task<int> AddOptionAsync(int questionId, CreateOptionDto request)
        {
            var question = await _questionRepository.GetByIdWithQuizAsync(questionId)
                ?? throw new KeyNotFoundException("Question not found.");
            if (question.Quiz.IsPublished)
                throw new InvalidOperationException("Cannot add an option to a question in a published quiz.");
            var option = new Option
            {
                Text = request.Text,
                IsCorrect = request.IsCorrect,
                QuestionId = questionId
            };

            await _opRepository.AddAsync(option);
            await _opRepository.SaveChangesAsync();
            return option.Id;

        }

        public async Task UpdateOptionAsync(int optionId, CreateOptionDto request)
        {
            var option =await  _optionRepository.GetByIdWithQuestionAndQuizAsync(optionId)
                ?? throw new KeyNotFoundException("Option not found.");
            if (option.Question.Quiz.IsPublished)
            {
                throw new InvalidOperationException("Cannot modify an option in a published quiz.");
            }
            
            option.Text = request.Text;
            option.IsCorrect = request.IsCorrect;

            await _opRepository.SaveChangesAsync();


        }

        public async Task DeleteOptionAsync(int optionId)
        {
            var option = await _optionRepository.GetByIdWithQuestionAndQuizAsync(optionId)
                ?? throw new KeyNotFoundException("Option not found.");
            if (option.Question.Quiz.IsPublished)
            {
                throw new InvalidOperationException("Cannot delete an option from a published quiz.");
            }

            _opRepository.DeleteAsync(option);
            await _opRepository.SaveChangesAsync();
        }
    }
}
