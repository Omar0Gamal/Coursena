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
        [HttpGet("courses/{code}")]
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
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _courseService.GetByIdAsync(id);

            if (result == null)
            {
                return Problem(
                    title: "Not Found",
                    detail: "Course does not exist or is not approved",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            return Ok(result);
        }
    }
}