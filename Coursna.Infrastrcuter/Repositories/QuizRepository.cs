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
        public async Task<Quiz?> GetQuizWithQuestionsAsync(int quizId, string teacherId)
        {
            var quiz= await _context.quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == quizId && q.course.TeacherId == teacherId);
            return quiz;
        }

        public async Task<List<Quiz>> GetQuizzesByCourseIdAsync(int courseId, string teacherId)
        {
           return await _context.quizzes.Where(q => q.CourseId == courseId&& q.course.TeacherId==teacherId).ToListAsync();
        }
        public async Task<List<Quiz>> GetPublishedByCourseIdAsync(int courseId)
        {
            var query = _context.quizzes.Where(q => q.CourseId == courseId && q.IsPublished);


            return await query.OrderByDescending(q => q.CreatedAt).ToListAsync();
        }
        public async Task<Quiz?> GetQuizForTeacherAsync(int quizId, string teacherId)
        {
            return await _context.quizzes
                .FirstOrDefaultAsync(q => q.Id == quizId && q.course.TeacherId == teacherId);
        }

        public async Task<Quiz> GetStudentQuizWithQuestionsAsync(int quizId)
        {
        var quiz = await _context.quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == quizId );
            return quiz;
        }
    }
}
