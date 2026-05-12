using Coursna.Core.Domain.Entities;
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
        Task<ApplicationUser> Register(UserRegisterDto request);
        Task<AuthResponseDto> Login(UserLoginDto request);
        Task<UserResponseDto> GetMyTeacherAsync(string studentId);
        Task<List<UserResponseDto>> GetMyStudentsAsync(string teacherId);
        Task<UserResponseDto> GetMeAsync(string userId);
    }
}


