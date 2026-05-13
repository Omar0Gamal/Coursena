using Coursna.Core.Domain.Entities;
using Coursna.Core.Domain.RepositoryInterface;
using Coursna.Core.Dtos;
using Coursna.Infrastrcuter.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Infrastrcuter.Repositories
{
    public class AttemptRepository : IAttemptRepository
    {
        private readonly AppDbContext _context;

        public AttemptRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<QuizAttempt> GetActiveAttemptAsync(int quizId, string studentId)
        {
            return await _context.QuizAttempts
                .Include(a => a.Quiz)
                .FirstOrDefaultAsync(a =>
                    a.QuizId == quizId &&
                    a.StudentId == studentId &&
                    a.Status == Core.Domain.Enums.AttemptStatus.InProgress);

        }

        public async Task<QuizAttempt> GetByIdWithFullQuizDataAsync(int id, string studentId)
        {
            return await _context.QuizAttempts
                 .Include(a => a.Responses)
                 .Include(a => a.Quiz)
                     .ThenInclude(q => q.Questions)
                         .ThenInclude(q => q.Options)
                 .FirstOrDefaultAsync(a => a.Id == id && a.StudentId == studentId);
        }

        public async Task<int> GetAttemptCountAsync(int quizId, string StudentId)
        {

            return await _context.QuizAttempts
                .CountAsync(a => a.QuizId == quizId && a.StudentId == StudentId);
        }

        public async Task<List<QuizAttempt>> GetQuizResultsAsync(int quizId)
        {
            return await _context.QuizAttempts
                .Include(a => a.Student)
                .Include(a => a.Responses)
                .Include(a => a.Quiz)
                    .ThenInclude(q => q.Questions)
                        .ThenInclude(q => q.Options)
                .Where(a => a.QuizId == quizId && a.Status == Core.Domain.Enums.AttemptStatus.Completed)
                .ToListAsync();
        }

        public async Task<List<QuizAttempt>> GetStudentAttemptsByCourseIdAsync(string studentId, int courseId)
        {
            return await _context.QuizAttempts
                .Include(a => a.Quiz)
                    .ThenInclude(q => q.Questions)
                .Where(a => a.StudentId == studentId && a.Quiz.CourseId == courseId && a.Status == Core.Domain.Enums.AttemptStatus.Completed)
                .ToListAsync();
        }
    }
}


