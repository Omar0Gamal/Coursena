using Coursna.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.ServiceContracts
{
    public interface ICourseService
    {
        Task<CourseResponseDto> CreateCourseAsync(CreateCourseDto dto, string teacherId);
        Task<List<CourseResponseDto>> GetTeacherCoursesAsync(string teacherId);
        Task<string> GetInviteCodeAsync(string teacherId);

        Task<CourseResponseDto?> GetByIdAsync(int id);

        Task<bool> UpdateCourseAsync(int id, CreateCourseDto dto, string teacherId);

        Task<bool> DeleteCourseAsync(int id, string teacherId);

        Task<List<CourseResponseDto>> GetPublicCoursesByInviteCodeAsync(string code);
        Task<List<CourseResponseDto>> GetAllCoursesAsync();

        Task<bool> ApproveCourseAsync(int id);

        Task<bool> RejectCourseAsync(int id);
        Task<List<CourseResponseDto>> SearchCoursesAsync(
            string inviteCode,
            string searchBy,
            string searchString);
        Task<List<CourseResponseDto>> GetCoursesForStudentAsync(string studentId, string inviteCode);
    }
    
}
