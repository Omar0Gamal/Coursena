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
    public class TeacherDashboardRepository : ITeacherDashboardRepository
    {
        private readonly AppDbContext _Context;
        public TeacherDashboardRepository(AppDbContext context)
        {
            _Context = context;
        }

        public async Task<int> GetActiveStudentsAsync(string teacherId)
        {
           return  await _Context.Enrollments.Where(e=>e.EndDate>DateTime.UtcNow&&e.course.TeacherId==teacherId).Select(e=>e.StudentId).Distinct().CountAsync();
        }

        public async Task<int> GetActiveCoursesAsync(string teacherId)
        {
            return await _Context.Courses
                .Where(c => c.TeacherId == teacherId && c.IsApproved)
                .CountAsync();
        }

        public async Task<int> GetTotalCodesAsync(string teacherId)
        {
            return await _Context.courseCodes.Where(c=>c.Course.TeacherId==teacherId).CountAsync();
        }

        public async Task<int> GetTotalCoursesAsync(string teacherId)
        {
           return await _Context.Courses.Where(c=>c.TeacherId == teacherId).CountAsync();
        }

        public async Task<int> GetTotalStudentsAsync(string teacherId)
        {
            return await _Context.Users
              .Where(u => u.TeacherId == teacherId && u.Role == "Student")
              .CountAsync();
        }

        public async Task<int> GetUsedCodesAsync(string teacherId)
        {
            return await _Context.courseCodes
            .Where(c => c.Course.TeacherId == teacherId && c.IsUsed)
            .CountAsync();
        }

        public async Task<decimal> GetMonthlyRevenueAsync(string teacherId)
        {
            var firstDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            return await _Context.Enrollments
                .Where(e => e.course.TeacherId == teacherId && e.StartDate >= firstDayOfMonth)
                .SumAsync(e => e.course.Price);
        }
    }
}
