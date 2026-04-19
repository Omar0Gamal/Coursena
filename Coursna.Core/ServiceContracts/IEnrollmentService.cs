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
        Task<AuthResponseDto> EnrollByCodeAsync(string studentId, string code);
        Task<List<CourseResponseDto>> GetMyCoursesAsync(string studentId);
        Task<AuthResponseDto> CheckCompletionAsync(string studentId, int courseId);
    }
}
