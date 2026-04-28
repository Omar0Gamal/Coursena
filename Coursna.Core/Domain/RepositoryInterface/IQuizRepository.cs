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
        Task<Quiz> GetQuizWithQuestionsAsync(int quizId);
        Task<List<Quiz>> GetQuizzesByCourseIdAsync(int courseId);
        
  


    }
}
