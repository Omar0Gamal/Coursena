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
    [Route("api/v1/student")]
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
        [HttpPost("enrollments")]
        public async Task<IActionResult> EnrollByCode( EnrollByCodeDto dto)
        {
            
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(studentId))
            {
                return Unauthorized("User not logged in");
            }

           
            var result = await _enrollmentService.EnrollByCodeAsync(
                studentId,
                
                dto.Code
            );


         
            return Ok(result);
        }

        [HttpGet("my-courses")]
        public async Task<IActionResult> GetMyCourses()
        {
            var studentId = _userManager.GetUserId(User);

            var result = await _enrollmentService.GetMyCoursesAsync(studentId);

            return Ok(result);
        }
        [HttpGet("courses/{courseId}/completion-status")]
        public async Task<IActionResult> CheckCompletion(int courseId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _enrollmentService.CheckCompletionAsync(studentId, courseId);

            return Ok(result);
        }
    }
}
