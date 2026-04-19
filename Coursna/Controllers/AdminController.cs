using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/admin")]
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

          
            [HttpPost("approve-teacher/{teacherId}")]
            public async Task<IActionResult> ApproveTeacher(string teacherId)
            {
                var result = await _adminService.ApproveTeacherAsync(teacherId);

                if (!result.IsSuccess)
                {
                    return Problem(
                        title: "Approve Teacher Failed",
                        detail: result.Message,
                        statusCode: StatusCodes.Status400BadRequest
                    );
                }

                return Ok(result);
            }

            
            [HttpDelete("reject-teacher/{teacherId}")]
            public async Task<IActionResult> RejectTeacher(string teacherId)
            {
                var result = await _adminService.RejectTeacherAsync(teacherId);

                if (!result.IsSuccess)
                {
                    return Problem(
                        title: "Reject Teacher Failed",
                        detail: result.Message,
                        statusCode: StatusCodes.Status400BadRequest
                    );
                }

                return Ok(result);
            }
        [HttpGet("Get-Users")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _adminService.GetUsersAsync();
            return Ok(result);
        }

       
        [HttpDelete("Delete{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _adminService.DeleteUserAsync(userId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

       
        [HttpPost("Add-User")]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            var result = await _adminService.CreateUserAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
    }

