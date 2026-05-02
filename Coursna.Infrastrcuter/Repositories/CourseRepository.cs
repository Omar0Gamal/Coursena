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
            return await _context.Courses.Include(c=>c.Teacher)
                .Where(c => c.IsApproved && c.TeacherId == teacherId)
                .ToListAsync();
        }

        public async Task<List<Course>> GetTeacherCousres(string id)
        {
           return await _context.Courses.Include(c => c.Teacher).Where(t=>t.TeacherId==id).ToListAsync();
        }
        public async Task<List<Course>> GetByGradeIdAsync(int gradeId,string teacherId)
        {
            return await _context.Courses.Include(c=>c.Teacher)
                .Where(c => c.IsApproved && c.GradeId == gradeId && c.TeacherId==teacherId)
                .ToListAsync();
        }
        public async Task<List<Course>> SearchCoursesAsync(
    string teacherId,
    string searchBy,
    string searchString)
        {
            var query = _context.Courses
                .Include(c => c.grade)
                .Include(c => c.subject)
                .Where(c => c.TeacherId == teacherId && c.IsApproved);

            if (string.IsNullOrWhiteSpace(searchString))
                return await query.ToListAsync();

            searchBy = searchBy?.ToLower();

            switch (searchBy)
            {
                case "title":
                    query = query.Where(c => c.Title.Contains(searchString));
                    break;

                case "grade":
                    query = query.Where(c => c.grade.Name.Contains(searchString));
                    break;

                case "subject":
                    query = query.Where(c => c.subject.Name.Contains(searchString));
                    break;

                default:
                    
                    query = query.Where(c =>
                        c.Title.Contains(searchString) ||
                        c.grade.Name.Contains(searchString) ||
                        c.subject.Name.Contains(searchString));
                    break;
            }
            
            return await query.ToListAsync();
        }
        public async Task<Course?> GetByIdWithTeacherAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<List<Course>> GetPendingCoursesAsync()
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Where(c => !c.IsApproved)
                .ToListAsync();
        }
    }
}
