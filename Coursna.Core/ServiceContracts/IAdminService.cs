using Coursna.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.ServiceContracts
{
    public interface IAdminService
    {
        Task<List<TeacherResponseDto>> GetPendingTeachersAsync();
        Task<ApiResponseDto> ApproveTeacherAsync(string teacherId);
        Task<ApiResponseDto> RejectTeacherAsync(string teacherId);
        Task<List<UserResponseDto>> GetUsersAsync();
        Task<ApiResponseDto> DeleteUserAsync(string userId);
        Task<ApiResponseDto> CreateUserAsync(CreateUserDto dto);
    }
}
