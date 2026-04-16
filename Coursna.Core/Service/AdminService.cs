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

namespace Coursna.Core.Service
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<AuthResponseDto> ApproveTeacherAsync(string teacherId)
        {
            var teacher= await _userManager.FindByIdAsync(teacherId);
            if (teacher == null)
            {
                return AuthResponseDto.Fail("Teacher not found");

            }
            if (teacher.IsApproved)
            {
                return AuthResponseDto.Fail("Teacher already approved");
            }
            teacher.IsApproved = true;
            teacher.InviteCode = GenerateInviteCode();
            var result=await _userManager.UpdateAsync(teacher);
            if (!result.Succeeded)
                return AuthResponseDto.Fail(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );

            return AuthResponseDto.Success(
                $"Teacher approved successfully. InviteCode: {teacher.InviteCode}"
            );

        }

        public async Task<List<TeacherResponseDto>> GetPendingTeachersAsync()
        {
            var teachers = await _userManager.Users.Where(t => t.IsApproved == false).ToListAsync();
            return teachers.Select(t=>t.ToTeacherResponse()).ToList();
        }

        public async Task<AuthResponseDto> RejectTeacherAsync(string teacherId)
        {
            var teacher = await _userManager.FindByIdAsync(teacherId);

            if (teacher == null)
                return AuthResponseDto.Fail("Teacher not found");

            var result = await _userManager.DeleteAsync(teacher);

            if (!result.Succeeded)
                return AuthResponseDto.Fail(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );

            return AuthResponseDto.Success("Teacher rejected successfully");
        }
        private string GenerateInviteCode()
        {
            return Guid.NewGuid().ToString("N")[..6].ToUpper();
        }
    }
}
