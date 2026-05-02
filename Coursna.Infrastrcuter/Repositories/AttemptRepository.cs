using Coursna.Core.Domain.Entities;
using Coursna.Core.Domain.RepositoryInterface;
using Coursna.Infrastrcuter.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Infrastrcuter.Repositories
{
    public class AttemptRepository:IAttemptRepository
    {
        private readonly AppDbContext _context;

        public AttemptRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<QuizAttempt> GetActiveAttemptAsync(int quizId, string studentId)
        {
            return await _context.QuizAttempts
                .FirstOrDefaultAsync(a =>
                    a.QuizId == quizId &&
                    a.StudentId == studentId &&
                    a.Status == Core.Domain.Enums.AttemptStatus.InProgress);

        }

        public async Task<QuizAttempt> GetByIdWithFullQuizDataAsync(int id)
        {
            return await _context.QuizAttempts
                 .Include(a => a.Responses)
                 .Include(a => a.Quiz)
                     .ThenInclude(q => q.Questions)
                         .ThenInclude(q => q.Options)
                 .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<int> GetAttemptCountAsync(int quizId, string StudentId)
        {

            return await _context.QuizAttempts
                .CountAsync(a => a.QuizId == quizId && a.StudentId == StudentId);
        }

    }
}
