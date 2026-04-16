using Coursna.Core.Dtos;
using Coursna.Core.Domain.IdentityEntities;
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

        public TeacherCourseController(
            ICourseService courseService,
            UserManager<ApplicationUser> userManager)
        {
            _courseService = courseService;
            _userManager = userManager;
        }

 
        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseDto dto)
        {
            var teacherId = _userManager.GetUserId(User);

            var result = await _courseService.CreateCourseAsync(dto, teacherId);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyCourses()
        {
            var teacherId = _userManager.GetUserId(User);

            var result = await _courseService.GetTeacherCoursesAsync(teacherId);

            return Ok(result);
        }


        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
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
    }
}