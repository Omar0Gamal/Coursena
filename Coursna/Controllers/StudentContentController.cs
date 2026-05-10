using Coursna.Core.Domain.Entities;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/v1/student/content")]
    [Authorize(Roles = "Student")]
    public class StudentContentController : ControllerBase
    {
        private readonly ICourseContentService _contentService;

        public StudentContentController(ICourseContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetContent(int courseId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);


                var result = await _contentService.GetCourseContentAsync(courseId, studentId);
                return Ok(result);
        
        }
    }
}


