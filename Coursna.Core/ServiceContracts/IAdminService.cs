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
        Task<AuthResponseDto> ApproveTeacherAsync(string teacherId);
        Task<AuthResponseDto> RejectTeacherAsync(string teacherId);
        Task<List<UserResponseDto>> GetUsersAsync();
        Task<AuthResponseDto> DeleteUserAsync(string userId);
        Task<AuthResponseDto> CreateUserAsync(CreateUserDto dto);
    }
}
