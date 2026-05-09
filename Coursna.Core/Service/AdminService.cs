using Coursna.Core.Domain.IdentityEntities;
using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coursna.Core.Exceptions;

namespace Coursna.Core.Service
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ApiResponseDto> ApproveTeacherAsync(string teacherId)
        {
            var teacher= await _userManager.FindByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new NotFoundException("Teacher not found");

            }
            if (teacher.IsApproved)
            {
                throw new BadRequestException("Teacher already approved");
            }
            teacher.IsApproved = true;
            teacher.InviteCode = GenerateInviteCode();
            var result=await _userManager.UpdateAsync(teacher);
            if (!result.Succeeded)
                throw new BadRequestException(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );

            return ApiResponseDto.Success(
                $"Teacher approved successfully. InviteCode: {teacher.InviteCode}"
            );

        }

        public async Task<List<TeacherResponseDto>> GetPendingTeachersAsync()
        {
            var teachers = await _userManager.Users.Where(t => t.IsApproved == false).ToListAsync();
            return teachers.Select(t=>t.ToTeacherResponse()).ToList();
        }

        public async Task<ApiResponseDto> RejectTeacherAsync(string teacherId)
        {
            var teacher = await _userManager.FindByIdAsync(teacherId);

            if (teacher == null)
                throw new NotFoundException("Teacher not found");

            var result = await _userManager.DeleteAsync(teacher);

            if (!result.Succeeded)
                throw new NotFoundException(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );

            return ApiResponseDto.Success("Teacher rejected successfully");
        }
        private string GenerateInviteCode()
        {
            return Guid.NewGuid().ToString("N")[..6].ToUpper();
        }

        public async Task<List<UserResponseDto>> GetUsersAsync()
        {
            var users = _userManager.Users.ToList();

            var result = new List<UserResponseDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new UserResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = roles.FirstOrDefault()
                });
            }

            return result;
        }

       
        public async Task<ApiResponseDto> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return ApiResponseDto.Fail("User not found");

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return ApiResponseDto.Fail(
                    string.Join(",", result.Errors.Select(e => e.Description))
                );

            return ApiResponseDto.Success("User deleted successfully");
        }

        public async Task<ApiResponseDto> CreateUserAsync(CreateUserDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return ApiResponseDto.Fail(
                    string.Join(",", result.Errors.Select(e => e.Description))
                );

          
            await _userManager.AddToRoleAsync(user, dto.Role);

            return ApiResponseDto.Success("User created successfully");
        }
    }
}
