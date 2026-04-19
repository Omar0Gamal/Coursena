using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class PublicCourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public PublicCourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

       
        [HttpGet]
        [AllowAnonymous]
        [HttpGet("GetTeacherCourses/{code}")]
        public async Task<IActionResult> GetCoursesByInviteCode(string code)
        {
          
            if (string.IsNullOrWhiteSpace(code))
            {
                return Problem(
                    title: "Invalid Code",
                    detail: "Invite code is required",
                    statusCode: 400
                );
            }

            var result = await _courseService.GetPublicCoursesByInviteCodeAsync(code);

           
            if (result == null || !result.Any())
            {
                return NotFound("No courses found for this teacher");
            }

           
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string inviteCode,string searchBy,string searchString)
        {
            var result = await _courseService.SearchCoursesAsync(
                inviteCode,
                searchBy,
                searchString);

            return Ok(result);
        }
    }
}