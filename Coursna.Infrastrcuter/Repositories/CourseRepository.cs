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

        public async Task<List<Course>> GetPublicCourses()
        {
            return await _context.Courses.Where(c=>c.IsApproved).ToListAsync();
        }

        public async Task<List<Course>> GetTeacherCousres(string id)
        {
           return await _context.Courses.Where(t=>t.TeacherId==id).ToListAsync();
        }
    }
}
