using Coursna.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.ServiceContracts
{
    public interface IAttemptService
    {
        Task<int> StartAttemptAsync(int quizId,string studentId);
       // Task <List<AttemptResponseDto>> GetActiveAttemptResponses(int attemptId, String studentId);
        Task SaveResponseAsync(int attemptId, SaveResponseRequest request,string studentId);
        Task<AttemptResultResponse> SubmitAttemptAsync(int attemptId, string studentId);
        Task<QuizWithQuestionsDto> GetAttemptQuestionsAsync(int attemptId, string studentId);
        Task<AttemptResultResponse> GetAttemptResultAsync(int attemptId, string studentId);
        Task<ActiveAttemptDto> GetActiveAttemptAsync(int quizId, string studentId);
        Task<List<AttemptResultResponse>> GetStudentAttemptsByCourseIdAsync(string studentId, int courseId);
    }
}
