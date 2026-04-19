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
        Task<AuthResponseDto> RegisterTeacherAsync(RegisterTeacherDto registerTeacherDto);
        Task<AuthResponseDto> RegisterStudentAsync(RegisterStudentDto registerStudentDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> LogoutAsync();
        Task<AuthResponseDto> Update(string userId, RegisterTeacherDto dto);
    }
}
