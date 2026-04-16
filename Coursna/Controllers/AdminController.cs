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

            // 👀 Get pending teachers
            [HttpGet("pending-teachers")]
            public async Task<IActionResult> GetPendingTeachers()
            {
                var result = await _adminService.GetPendingTeachersAsync();
                return Ok(result);
            }

            // ✅ Approve teacher
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

            // ❌ Reject teacher
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
        }
    }

