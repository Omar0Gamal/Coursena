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
        public Task<QuizResponseDto> CreateQuizAsync(CreateQuizDto dto, string teacherId);
        Task<List<QuizResponseDto>> GetQuizzesByCourseIdAsync(int courseId, string teacherId);
        Task<QuizWithQuestionsDto> GetStudentQuizWithQuestionsByIdAsync(int quizId, string studentId);
        Task<QuizWithQuestionWithAnswersDto> GetTeacherQuizWithQuestionsByIdAsync(int quizId, string teacherId);
        Task<QuizWithAnswersDto> GetTeacherQuizWithAnswersByIdAsync(int quizId, string teacherId);

        Task<QuizResponseDto?> GetQuizByIdAsync(int quizId);
        public Task<QuizResponseDto> UpdateQuizAsync(int quizId, CreateQuizDto dto, string teacherId);
        Task<bool> DeleteQuizAsync(int quizId, string teacherId);
        public  Task PublishQuizAsync(int quizId, string teacherId);  
        Task<int> AddQuestionAsync(int quizId, CreateQuestionDto question, string teacherId);
        public Task<List<PublishedQuizDto>> GetPublishedQuizzesByCourseIdAsyc(int courseId, string studentId);
        Task<List<QuizResultDto>> GetQuizResultsAsync(int quizId, string teacherId);
    }
}
