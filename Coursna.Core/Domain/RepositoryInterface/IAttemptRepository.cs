using Coursna.Core.Domain.Entities;
using Coursna.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Domain.RepositoryInterface
{
    public interface IAttemptRepository
    {
        Task<QuizAttempt> GetByIdWithFullQuizDataAsync(int id,string studentId);
        Task<QuizAttempt> GetActiveAttemptAsync(int quizId, String StudentId);
        Task<int> GetAttemptCountAsync(int quizId, String studentId);
    }
}
