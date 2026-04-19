using Coursna.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.ServiceContracts
{
    public interface ITeacherDashboardService
    {
        Task<TeacherDashboardDto> GetDashboardAsync(string teacherId);
    }
}
