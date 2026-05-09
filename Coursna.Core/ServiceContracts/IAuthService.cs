using Coursna.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Contracts
{
    public interface IAuthService
    {
        Task<ApiResponseDto> RegisterTeacherAsync(RegisterTeacherDto registerTeacherDto);
        Task<ApiResponseDto> RegisterStudentAsync(RegisterStudentDto registerStudentDto);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<ApiResponseDto> LogoutAsync();
        Task<ApiResponseDto> Update(string userId, RegisterTeacherDto dto);
        Task<MeDto> GetCurrentUserAsync(string userId);
    }
}
