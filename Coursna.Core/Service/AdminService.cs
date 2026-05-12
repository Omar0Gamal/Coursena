using Coursna.Core.Domain.Entities;
using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coursna.Core.Exceptions;
using Coursna.Core.Contracts;

namespace Coursna.Core.Service
{
    public class AdminService : IAdminService
    {
        private readonly IAuthRepository _authRepo;

        public AdminService(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        public async Task<ApiResponseDto> ApproveTeacherAsync(string teacherId)
        {
            var teacher = await _authRepo.GetUserByIdAsync(teacherId);
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

            await _authRepo.UpdateUserAsync(teacher);

            return ApiResponseDto.Success(
                $"Teacher approved successfully. InviteCode: {teacher.InviteCode}"
            );
        }

        public async Task<List<TeacherResponseDto>> GetPendingTeachersAsync()
        {
            var teachers = await _authRepo.GetPendingTeachersAsync();

            // ToTeacherResponse() mapping extension should be called
            return teachers.Select(t => new TeacherResponseDto
            {
                Id = t.Id,
                Email = t.Email,
                FullName = t.FullName,
                IsApproved = t.IsApproved
            }).ToList();
        }

        public async Task<ApiResponseDto> RejectTeacherAsync(string teacherId)
        {
            var teacher = await _authRepo.GetUserByIdAsync(teacherId);

            if (teacher == null)
                throw new NotFoundException("Teacher not found");

            await _authRepo.DeleteUserAsync(teacher);

            return ApiResponseDto.Success("Teacher rejected successfully");
        }

        private string GenerateInviteCode()
        {
            return Guid.NewGuid().ToString("N")[..6].ToUpper();
        }

        public async Task<List<UserResponseDto>> GetUsersAsync()
        {
            var users = await _authRepo.GetAllUsersAsync();

            var result = new List<UserResponseDto>();

            foreach (var user in users)
            {
                result.Add(new UserResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role
                });
            }

            return result;
        }

        public async Task<ApiResponseDto> DeleteUserAsync(string userId)
        {
            var user = await _authRepo.GetUserByIdAsync(userId);

            if (user == null)
                return ApiResponseDto.Fail("User not found");

            await _authRepo.DeleteUserAsync(user);

            return ApiResponseDto.Success("User deleted successfully");
        }

        public async Task<ApiResponseDto> CreateUserAsync(CreateUserDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                Role = dto.Role,
                IsApproved = true
            };

            await _authRepo.Register(user, dto.Password);

            return ApiResponseDto.Success("User created successfully");
        }

        public async Task<StateDto> GetStatsAsync()
        {
            var stats = await _authRepo.GetStatsAsync();
            return new StateDto
            {
                TotalUsers = stats.TotalUsers,
                TotalTeachers = stats.TotalTeachers,
                TotalCourses = stats.TotalCourses,
                PendingTeachers = stats.PendingTeachers,
                PendingCourses = stats.PendingCourses
            };
        }
    }
}


