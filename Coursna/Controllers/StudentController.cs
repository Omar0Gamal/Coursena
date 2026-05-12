using Coursna.Core.Domain.Entities;
using Coursna.Core.Contracts;
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
        private readonly IAuthService _authService;
        private readonly ICourseService _courseService;

        public StudentController(IEnrollmentService enrollmentService, IAuthService authService, ICourseService courseService)
        {
            _enrollmentService = enrollmentService;
            _authService = authService;
            _courseService = courseService;
        }

        [HttpGet("my-teacher")]
        public async Task<IActionResult> GetMyTeacher()
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _authService.GetMyTeacherAsync(studentId);
            return Ok(result);
        }

        [HttpPost("enrollments")]
        public async Task<IActionResult> EnrollByCode(EnrollByCodeDto dto)
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

        /// <summary>
        /// Gets the full content of a course for an enrolled student.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>Detailed course content including video and documents.</returns>
        [HttpGet("courses/{courseId}/content")]
        [ProducesResponseType(typeof(CourseDetailsResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetContent(int courseId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _courseService.GetCourseContentAsync(courseId, studentId);
            return Ok(result);
        }
    }
}
