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
    public class CourseRepository:IcourseRepository
    {
        private readonly AppDbContext _context;
        public CourseRepository(AppDbContext context)  { 
        _context = context;
        }

        public async Task<List<Course>> GetPublicCoursesByTeacherAsync(string teacherId)
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.grade)
                .Include(c => c.subject)
                .Where(c => c.IsApproved && c.TeacherId == teacherId)
                .ToListAsync();
        }

        public async Task<List<Course>> GetTeacherCousres(string id)
        {
           return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.grade)
                .Include(c => c.subject)
                .Where(t => t.TeacherId == id)
                .ToListAsync();
        }
        public async Task<List<Course>> GetByGradeIdAsync(int gradeId)
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.grade)
                .Include(c => c.subject)
                .Where(c => c.IsApproved && c.GradeId == gradeId)
                .ToListAsync();
        }
        public async Task<List<Course>> SearchAsync(
       string? teacherId,
       int? gradeId,
       bool isPublic,
       string? searchBy,
       string? searchString)
        {
            var query = _context.Courses
                .Include(c => c.grade)
                .Include(c => c.subject)
                .Include(c => c.Teacher)
                .AsQueryable();

            if (isPublic)
            {
                query = query.Where(c => c.IsApproved);
            }
            else if (teacherId != null)
            {
                query = query.Where(c => c.TeacherId == teacherId && c.IsApproved);
            }
            else if (gradeId != null)
            {
                query = query.Where(c => c.GradeId == gradeId && c.IsApproved);
            }

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchBy = searchBy?.ToLower();

                query = searchBy switch
                {
                    "subject" => query.Where(c => c.subject.Name.Contains(searchString)),
                    "title" => query.Where(c => c.Title.Contains(searchString)),
                    "grade" => query.Where(c => c.grade.Name.Contains(searchString)),
                    _ => query.Where(c =>
                        c.Title.Contains(searchString) ||
                        c.subject.Name.Contains(searchString) ||
                        c.grade.Name.Contains(searchString))
                };
            }

            return await query.ToListAsync();
        }
        public async Task<Course?> GetByIdWithTeacherAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.grade)
                .Include(c => c.subject)
                .Include(c => c.quizzes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<List<Course>> GetPendingCoursesAsync()
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.grade)
                .Include(c => c.subject)
                .Where(c => !c.IsApproved)
                .ToListAsync();
        }
    }
}
