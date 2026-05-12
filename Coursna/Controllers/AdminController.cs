using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/v1/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {

            private readonly IAdminService _adminService;

            public AdminController(IAdminService adminService)
            {
                _adminService = adminService;
            }

          
            [HttpGet("pending-teachers")]
            public async Task<IActionResult> GetPendingTeachers()
            {
                var result = await _adminService.GetPendingTeachersAsync();
                return Ok(result);
            }

          
            [HttpPost("teachers/{teacherId}/approve")]
            public async Task<IActionResult> ApproveTeacher(string teacherId)
            {
                var result = await _adminService.ApproveTeacherAsync(teacherId);



                return Ok(result);
            }

            
            [HttpPost("teachers/{teacherId}/reject")]
            public async Task<IActionResult> RejectTeacher(string teacherId)
            {
                var result = await _adminService.RejectTeacherAsync(teacherId);


                return Ok(result);
            }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _adminService.GetUsersAsync();
            return Ok(result);
        }

       
        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _adminService.DeleteUserAsync(userId);


            return Ok(result);
        }

       
        [HttpPost("users")]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            var result = await _adminService.CreateUserAsync(dto);

          
            return Ok(result);
        }
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await _adminService.GetStatsAsync();
            return Ok(result);
        }
    }
}

