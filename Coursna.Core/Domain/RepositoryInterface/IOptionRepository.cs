using Coursna.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Domain.RepositoryInterface
{
    public interface IOptionRepository
    {
        public Task<Option?> GetByIdWithQuestionAndQuizAsync(int id);
    }
}
