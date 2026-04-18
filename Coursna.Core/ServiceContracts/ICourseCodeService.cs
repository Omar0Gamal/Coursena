using Coursna.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.ServiceContracts
{
    public interface ICourseCodeService
    {
        Task<AuthResponseDto> GenerateCodesAsync(int courseId, int count);
        Task<List<CourseCodeResponseDto>> GetCodesAsync(string teacherId, int courseId);
    }
}
