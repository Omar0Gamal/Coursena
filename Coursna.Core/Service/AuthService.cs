using Coursna.Core.Contracts;
using Coursna.Core.Domain.IdentityEntities;
using Coursna.Core.Dtos;
using Coursna.Core.Exceptions;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;
        public AuthService(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }
        public async Task<ApiResponseDto> RegisterTeacherAsync(RegisterTeacherDto registerTeacherDto)
        {
            var existingUser= await _userManager.FindByEmailAsync(registerTeacherDto.Email);
            if (existingUser != null)
            {
                throw new BadRequestException("Email already exists");
            }
            var user = new ApplicationUser
            {
                UserName = registerTeacherDto.Email,
                Email = registerTeacherDto.Email,
                FullName = registerTeacherDto.FullName,
                IsApproved = false
            };
            var result= await _userManager.CreateAsync(user,registerTeacherDto.Password);
            if (!result.Succeeded)
            {
                throw new BadRequestException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            await _userManager.AddToRoleAsync(user, "Teacher");
            return new ApiResponseDto { IsSuccess = true, Message = "Teacher registered successfully, waiting for approval" };
            
        }

        public async Task<ApiResponseDto> RegisterStudentAsync(RegisterStudentDto registerStudentDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerStudentDto.Email);
            if (existingUser != null)
            {
                throw new BadRequestException("Email already exists");
            }

            // Hna bn3mel check an el student da el teacher bta3o mawgod
            var teacher =  _userManager.Users.FirstOrDefault(t=>t.InviteCode == registerStudentDto.InviteCode);
            if (teacher == null)
            {
                throw new NotFoundException("Invalid TeacherId");
            }

            var user = new ApplicationUser
            {
                UserName = registerStudentDto.Email,
                Email = registerStudentDto.Email,
                FullName = registerStudentDto.FullName,
                TeacherId =teacher.Id,
                IsApproved = true, // el student m4 m7tag admin approval 
                gradeId=registerStudentDto.GradeId
            };

            var result = await _userManager.CreateAsync(user, registerStudentDto.Password);

            if (!result.Succeeded)
            {
                throw new BadRequestException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await _userManager.AddToRoleAsync(user, "Student");

            return new ApiResponseDto
            {
                IsSuccess = true,
                Message = "Student registered successfully"
            };
        }
        

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new NotFoundException("Invalid email or password");
            }
            var vaildPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!vaildPassword)
            {
                throw new BadRequestException("Invalid email or password");
            }
            if (!user.IsApproved) {
                throw new BadRequestException("Account not approved yet");
            }
            var token = await _jwtService.CreateJwtToken(user);

            var roles = await _userManager.GetRolesAsync(user);

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = roles.FirstOrDefault()
            };
          
        }

        public async Task<ApiResponseDto> LogoutAsync()
        {
            await _signInManager.SignOutAsync();

            return ApiResponseDto.Success("Loged out \n"); 
                
         }

        public async Task<ApiResponseDto> Update(string userId,RegisterTeacherDto dto)
        {
            var user=await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                throw new NotFoundException("Null user");
            }
            if (!string.IsNullOrWhiteSpace(dto.FullName))
            {
                user.FullName = dto.FullName;
            }
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var result = await _userManager.ResetPasswordAsync(user, token, dto.Password);

                if (!result.Succeeded)
                {
                    return ApiResponseDto.Fail(
                        string.Join(",", result.Errors.Select(e => e.Description))
                    );
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                throw new BadRequestException(string.Join(",", updateResult.Errors.Select(e => e.Description)));
            }

            return ApiResponseDto.Success("User updated successfully");
        }
        public async Task<MeDto> GetCurrentUserAsync(string userId)
        {
            var user = await _userManager.Users
                .Include(u => u.Teacher)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var role = roles.FirstOrDefault();

            return new MeDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = role,
                IsApproved = user.IsApproved,

                TeacherId = user.TeacherId,

                TeacherName = user.Teacher?.FullName,

                InviteCode = role == "Teacher"
                    ? user.InviteCode
                    : user.Teacher?.InviteCode
            };
        }

    }
}
