using Coursna.Core.Domain.Entities;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/v1/courses")]
    public class PublicCourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public PublicCourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("{inviteCode}/courses")]
        public async Task<IActionResult> GetPublicCourses(string inviteCode)
        {
            var result = await _courseService
                .GetPublicCoursesByInviteCodeAsync(inviteCode);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses() //Registerd k student
        {
            if (User.IsInRole("Student"))
            {
                var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _courseService
                    .GetCoursesForStudentAsync(studentId);

                return Ok(result);
            }

            return Ok();
        }

        [Authorize]
        [HttpGet("search")]
            public async Task<IActionResult> Search(string searchBy, string searchString)
            {

                // Student
                if (User.IsInRole("Student"))
                {
                    var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var result = await _courseService.SearchStudentCoursesAsync(studentId, searchBy, searchString);
                    return Ok(result);
                }

                // Teacher (optional)
                if (User.IsInRole("Teacher"))
                {
                    var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var result = await _courseService.SearchTeacherCoursesAsync(teacherId, searchBy, searchString);
                    return Ok(result);
                }

                return Ok();
            }
        [HttpGet("public/{inviteCode}/search")]
        public async Task<IActionResult> PublicSearch(
    string inviteCode,
    string searchBy,
    string searchString)
        {
            var result = await _courseService.SearchPublicByTeacherAsync(
                inviteCode,
                searchBy,
                searchString);

            return Ok(result);
        }
    }
}

