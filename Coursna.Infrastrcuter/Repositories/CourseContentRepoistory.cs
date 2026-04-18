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
    public class CourseContentRepository : Repository<CourseContent>, ICourseContentRepository
    {
        private readonly AppDbContext _context;

        public CourseContentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CourseContent>> GetByCourseIdAsync(int courseId)
        {
            return await _context.CourseContents
                .Where(c => c.CourseId == courseId)
                .OrderBy(c => c.order)
                .ToListAsync();
        }
    }
}
