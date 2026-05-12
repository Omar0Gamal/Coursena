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
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        private readonly AppDbContext _Context;
        public EnrollmentRepository(AppDbContext context):base(context) 
        {
            _Context = context;
        }
        public async Task<Enrollment?> GetActiveEnrollmentAsync(string studentId, int courseId)
        {
            return await _Context.Enrollments.FirstOrDefaultAsync(e =>
                e.StudentId == studentId &&
                e.CourseId == courseId &&
                e.EndDate > DateTime.UtcNow);
        }

        public async Task<List<Course>> GetStudentCoursesAsync(string studentId)
        {
            return await _Context.Enrollments
           .Where(e => e.StudentId == studentId)
           .Include(e => e.course).ThenInclude(c => c.Teacher)
           .Include(e => e.course).ThenInclude(c => c.grade)
           .Include(e => e.course).ThenInclude(c => c.subject)
           .Select(e => e.course)
           .ToListAsync();
        }
        public async Task<Enrollment?> GetEnrollmentAsync(string studentId, int courseId)
        {
            return await _Context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }
    }
}
