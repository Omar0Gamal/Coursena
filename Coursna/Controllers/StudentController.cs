using Coursna.Core.Domain.IdentityEntities;
using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/student")]
    [Authorize(Roles = "Student")]
    public class StudentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(IEnrollmentService enrollmentService, UserManager<ApplicationUser> userManager)
        
        {
            _enrollmentService = enrollmentService;
                _userManager = userManager;
        }
        [HttpPost("enroll-by-code")]
        public async Task<IActionResult> EnrollByCode( EnrollByCodeDto dto)
        {
            
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(studentId))
            {
                return Unauthorized("User not logged in");
            }

           
            var result = await _enrollmentService.EnrollByCodeAsync(
                studentId,
                dto.CourseId,
                dto.Code
            );

         
            if (!result.IsSuccess)
            {
                return Problem(
                    title: "Enrollment Failed",
                    detail: result.Message,
                    statusCode: 400
                );
            }

         
            return Ok(result);
        }

        [HttpGet("my-courses")]
        public async Task<IActionResult> GetMyCourses()
        {
            var studentId = _userManager.GetUserId(User);

            var result = await _enrollmentService.GetMyCoursesAsync(studentId);

            return Ok(result);
        }
    }
}
