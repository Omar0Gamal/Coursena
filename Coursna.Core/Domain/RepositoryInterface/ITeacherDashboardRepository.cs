using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Domain.RepositoryInterface
{
    public interface ITeacherDashboardRepository
    {
        Task<int> GetTotalCoursesAsync(string teacherId);
        Task<int> GetTotalCodesAsync(string teacherId);
        Task<int> GetUsedCodesAsync(string teacherId);
        Task<int> GetTotalStudentsAsync(string teacherId);
        Task<int> GetActiveStudentsAsync(string teacherId);
    }
}
