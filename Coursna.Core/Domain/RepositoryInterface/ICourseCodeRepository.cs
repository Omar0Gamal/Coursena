using Coursna.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Domain.RepositoryInterface
{
    public interface ICourseCodeRepository: IRepository<CourseCode>
    {
        Task<CourseCode?> GetByCodeAsync(string code);
        Task AddRangeAsync(IEnumerable<CourseCode> codes);
        Task<List<CourseCode>> GetByCourseIdAsync(int courseId);
    }
}
