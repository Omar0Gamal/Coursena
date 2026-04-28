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
    public class OptionRepository : IOptionRepository
    {
        private readonly AppDbContext _context;
        public OptionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Option?> GetByIdWithQuestionAndQuizAsync(int id)
        {
            var option = await _context.Options.Include(o => o.Question).ThenInclude(q => q.Quiz).FirstOrDefaultAsync(o => o.Id == id);
            return option;
        }
    }
}
