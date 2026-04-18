using Coursna.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.ServiceContracts
{
    public interface IEnrollmentService
    {
        Task<AuthResponseDto> EnrollByCodeAsync(string studentId, int courseId, string code);
        Task<List<CourseResponseDto>> GetMyCoursesAsync(string studentId);
    }
}
