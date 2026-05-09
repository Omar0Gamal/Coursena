using Coursna.Core.Domain.IdentityEntities;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class PublicCourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly UserManager<ApplicationUser> _userManager;
        public PublicCourseController(ICourseService courseService, UserManager<ApplicationUser> user)
        {
            _courseService = courseService;
            _userManager = user;
        }

        [HttpGet("{inviteCode}/courses")]
        public async Task<IActionResult> GetPublicCourses(string inviteCode)  //anonymous
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
                var studentId = _userManager.GetUserId(User);

                var result = await _courseService
                    .GetCoursesForStudentAsync(studentId);

                return Ok(result);
            }

            return Ok();
        }


            [HttpGet("search")]
            public async Task<IActionResult> Search(string searchBy, string searchString)
            {

                // Student
                if (User.IsInRole("Student"))
                {
                    var studentId = _userManager.GetUserId(User);

                    var result = await _courseService.SearchStudentCoursesAsync(studentId, searchBy, searchString);
                    return Ok(result);
                }

                // Teacher (optional)
                if (User.IsInRole("Teacher"))
                {
                    var teacherId = _userManager.GetUserId(User);

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