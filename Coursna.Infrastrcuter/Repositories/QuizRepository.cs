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
    public class QuizRepository : IQuizRepository
    {

        private readonly AppDbContext _context;
        public QuizRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Quiz?> GetQuizWithQuestionsAsync(int quizId)
        {
            var quiz= await _context.quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == quizId);
            return quiz;
        }

        public async Task<List<Quiz>> GetQuizzesByCourseIdAsync(int courseId)
        {
           return await _context.quizzes.Where(q => q.CourseId == courseId).ToListAsync();
        }

    }
}
