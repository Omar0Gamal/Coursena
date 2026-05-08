using Coursna.Core.Domain.IdentityEntities;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/v1/courses")]
    public class PublicCourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly UserManager<ApplicationUser> _userManager;
        public PublicCourseController(ICourseService courseService,UserManager<ApplicationUser> user)
        {
            _courseService = courseService;
            _userManager = user;
        }

       

        [HttpGet]
        public async Task<IActionResult> GetCourses(string? inviteCode)
        {
            //  Anonymous
            if (!User.Identity.IsAuthenticated)
            {
                var result = await _courseService.GetPublicCoursesByInviteCodeAsync(inviteCode);
                return Ok(result);
            }

            // Student
            if (User.IsInRole("Student"))
            {
                var studentId =_userManager.GetUserId(User);

                
                if (string.IsNullOrEmpty(inviteCode))
                    return BadRequest("Invite code is required");

                var result = await _courseService
                    .GetCoursesForStudentAsync(studentId, inviteCode);

                return Ok(result);
            }

            return Ok(await _courseService.GetPublicCoursesByInviteCodeAsync(inviteCode));
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