using Coursna.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.ServiceContracts
{
    public interface IQuizService
    {
        Task<QuizResponseDto> CreateQuizAsync(CreateQuizDto dto, int courseId);
        Task<List<QuizResponseDto>> GetQuizzesByCourseIdAsync(int courseId);
        Task<QuizWithQuestionsDto> GetQuizWithQuestionsByIdAsync(int quizId);
        Task<QuizResponseDto?> GetQuizByIdAsync(int quizId);
        Task<QuizResponseDto> UpdateQuizAsync(int quizId, CreateQuizDto dto);
        Task<bool> DeleteQuizAsync(int quizId);
        Task PublishQuizAsync(int quizId);
        Task AddQuestionAsync(int quizId, QuestionDto question);
    }
}
