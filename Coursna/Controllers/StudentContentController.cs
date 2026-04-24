using Coursna.Core.Domain.IdentityEntities;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/student/content")]
    [Authorize(Roles = "Student")]
    public class StudentContentController : ControllerBase
    {
        private readonly ICourseContentService _contentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentContentController(ICourseContentService contentService, UserManager<ApplicationUser> userManager)
        {
            _contentService = contentService;
            _userManager = userManager;
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetContent(int courseId)
        {
            var studentId = _userManager.GetUserId(User);


                var result = await _contentService.GetCourseContentAsync(courseId, studentId);
                return Ok(result);
        
        }
    }
}
