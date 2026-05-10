using Coursna.Core.Domain.Entities;
using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public StudentController(IEnrollmentService enrollmentService)
        
        {
            _enrollmentService = enrollmentService;
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
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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


