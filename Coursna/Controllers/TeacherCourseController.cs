using Coursna.Core.Domain.IdentityEntities;
using Coursna.Core.Dtos;
using Coursna.Core.Service;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/teacher/courses")]
    [Authorize(Roles = "Teacher")]
    public class TeacherCourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITeacherDashboardService _teacherDashboardService;
        private readonly ILookUpService _lookupService;

        public TeacherCourseController(
            ICourseService courseService,
            UserManager<ApplicationUser> userManager,ITeacherDashboardService teacherDashboardService,ILookUpService lookUpService)
        {
            _courseService = courseService;
            _userManager = userManager;
            _teacherDashboardService = teacherDashboardService;
            _lookupService=lookUpService;

        }

 
        [HttpPost("Add")]
        public async Task<IActionResult> Create(CreateCourseDto dto)
        {
            var teacherId = _userManager.GetUserId(User);

            var result = await _courseService.CreateCourseAsync(dto, teacherId);

            return Ok(result);
        }

        [HttpGet("Get-Courses")]
        public async Task<IActionResult> GetMyCourses()
        {
            var teacherId = _userManager.GetUserId(User);

            var result = await _courseService.GetTeacherCoursesAsync(teacherId);

            return Ok(result);
        }


        [HttpPut("Update{id}")]
        public async Task<IActionResult> Update(int id, CreateCourseDto dto)
        {
            var teacherId = _userManager.GetUserId(User);

            var success = await _courseService.UpdateCourseAsync(id, dto, teacherId);

            if (!success)
            {
                return Problem(
                    title: "Update Failed",
                    detail: "Course not found or you are not allowed to update it",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }

            return Ok("Updated successfully");
        }

        [HttpDelete("Delete{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var teacherId = _userManager.GetUserId(User);

            var success = await _courseService.DeleteCourseAsync(id, teacherId);

            if (!success)
            {
                return Problem(
                    title: "Delete Failed",
                    detail: "Course not found or you are not allowed to delete it",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }

            return Ok("Deleted successfully");
        }
        [HttpGet("invite-code")]
        public async Task<IActionResult> GetInviteCode()
        {
            
            var teacherId = _userManager.GetUserId(User);

            var code = await _courseService.GetInviteCodeAsync(teacherId);

            if (string.IsNullOrEmpty(code))
            {
                return NotFound("Invite code not found");
            }

            return Ok(new { inviteCode = code });
        }
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var teacherId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(teacherId))
                return Unauthorized();

            var result = await _teacherDashboardService.GetDashboardAsync(teacherId);

            return Ok(result);
        }
       
        [HttpGet("subjects")]
        public async Task<IActionResult> GetSubjects()
        {
            var result = await _lookupService.GetSubjectsAsync();
            return Ok(result);
        }

        
        [HttpGet("grades")]
        public async Task<IActionResult> GetGrades()
        {
            var result = await _lookupService.GetGradesAsync();
            return Ok(result);
        }
    }
}