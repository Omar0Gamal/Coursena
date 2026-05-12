using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/v1/teacher/course")]
    [Authorize(Roles = "Teacher")]
    public class TeacherCourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ITeacherDashboardService _dashboardService;

        public TeacherCourseController(ICourseService courseService, ITeacherDashboardService dashboardService)
        {
            _courseService = courseService;
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Gets the dashboard statistics for the teacher.
        /// </summary>
        /// <returns>Dashboard metrics including total students, active courses, etc.</returns>
        [HttpGet("dashboard")]
        [ProducesResponseType(typeof(TeacherDashboardDto), 200)]
        public async Task<IActionResult> GetDashboard()
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _dashboardService.GetDashboardAsync(teacherId);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new course.
        /// </summary>
        /// <param name="dto">The course creation details.</param>
        /// <returns>The created course summary.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CourseResponseDto), 200)]
        public async Task<IActionResult> Create(CreateCourseDto dto)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _courseService.CreateCourseAsync(dto, teacherId);
            return Ok(result);
        }

        /// <summary>
        /// Gets all courses owned by the current teacher.
        /// </summary>
        /// <returns>A list of courses.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<CourseResponseDto>), 200)]
        public async Task<IActionResult> GetTeacherCourses()
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _courseService.GetTeacherCoursesAsync(teacherId);
            return Ok(result);
        }

        /// <summary>
        /// Gets the full details of a specific course owned by the teacher.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>Detailed course information including content and quizzes.</returns>
        [HttpGet("{courseId}")]
        [ProducesResponseType(typeof(CourseDetailsResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCourseDetails(int courseId)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _courseService.GetCourseDetailsAsync(courseId, teacherId);
            return Ok(result);
        }

        /// <summary>
        /// Updates a course's basic information.
        /// </summary>
        /// <param name="id">The course ID.</param>
        /// <param name="dto">The updated course details.</param>
        /// <returns>Success status.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, CreateCourseDto dto)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _courseService.UpdateCourseAsync(id, dto, teacherId);
            return Ok("Course updated successfully");
        }

        /// <summary>
        /// Deletes a course.
        /// </summary>
        /// <param name="id">The course ID.</param>
        /// <returns>Success status.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _courseService.DeleteCourseAsync(id, teacherId);
            return Ok("Course deleted successfully");
        }

        /// <summary>
        /// Gets the invitation code for the teacher's profile.
        /// </summary>
        /// <returns>The invitation code wrapped in a JSON object.</returns>
        [HttpGet("invite-code")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> GetInviteCode()
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _courseService.GetInviteCodeAsync(teacherId);
            return Ok(new { inviteCode = result });
        }

        /// <summary>
        /// Gets all students enrolled in a specific course.
        /// </summary>
        /// <param name="courseId">The course ID.</param>
        /// <returns>A list of students.</returns>
        [HttpGet("{courseId}/enrollments")]
        [ProducesResponseType(typeof(List<UserResponseDto>), 200)]
        public async Task<IActionResult> GetCourseEnrollments(int courseId)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _courseService.GetCourseEnrollmentsAsync(teacherId, courseId);
            return Ok(result);
        }

        /// <summary>
        /// Updates the content (video, text, documents) of a course.
        /// </summary>
        /// <param name="dto">The updated content details.</param>
        /// <returns>Success status.</returns>
        [HttpPut("content")]
        [ProducesResponseType(typeof(ApiResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateContent(UpdateCourseContentDto dto)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _courseService.UpdateCourseContentAsync(dto, teacherId);
            return Ok(result);
        }
    }
}
