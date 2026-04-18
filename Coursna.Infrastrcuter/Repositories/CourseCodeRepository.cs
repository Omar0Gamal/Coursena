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
    public class CourseCodeRepository : Repository<CourseCode>, ICourseCodeRepository
    {
    
        private readonly AppDbContext _context;
        public CourseCodeRepository(AppDbContext context): base(context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<CourseCode> codes)
        {
            await _context.courseCodes.AddRangeAsync(codes);
            await _context.SaveChangesAsync();
        }

        public async Task<CourseCode?> GetByCodeAsync(string code)
        {
           return await _context.courseCodes.FirstOrDefaultAsync(c=>c.Code == code);
        }
        public async Task<List<CourseCode>> GetByCourseIdAsync(int courseId)
        {
            return await _context.courseCodes
                .Where(c => c.CourseId == courseId)
                .ToListAsync();
        }
    }
}
