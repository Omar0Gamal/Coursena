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
    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _context;
        public QuestionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Question?> GetByIdWithQuizAsync(int id)
        {
          var question = await _context.Questions.Include(q => q.Quiz).FirstOrDefaultAsync(q => q.Id == id);
            return question;
        }
    }
}
