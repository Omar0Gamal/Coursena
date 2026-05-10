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
    [Route("api/v1/teacher/content")]
    [Authorize(Roles = "Teacher")]
    public class TeacherContentController : ControllerBase
    {
        private readonly ICourseContentService _contentService;

        public TeacherContentController(ICourseContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpPost]
        public async Task<IActionResult> AddContent(CreateContentDto dto)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _contentService.AddContentAsync(dto, teacherId);


            return Ok(result);
        }
    }
}


