using Coursna.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Domain.RepositoryInterface
{
    public interface IQuizRepository
    {
        Task<Quiz> GetQuizWithQuestionsAsync(int quizId, string teacherId);
        Task<Quiz> GetStudentQuizWithQuestionsAsync(int quizId);

        Task<List<Quiz>> GetQuizzesByCourseIdAsync(int courseId, string teacherId);
        Task<List<Quiz>> GetPublishedByCourseIdAsync(int courseId);
        public Task<Quiz?> GetQuizForTeacherAsync(int quizId, string teacherId);




    }
}
