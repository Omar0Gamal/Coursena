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
    [Route("api/v1/teacher/content")]
    [Authorize(Roles = "Teacher")]
    public class TeacherContentController : ControllerBase
    {
        private readonly ICourseContentService _contentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeacherContentController(ICourseContentService contentService,UserManager<ApplicationUser> userManager)
        {
            _contentService = contentService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddContent(CreateContentDto dto)
        {
            var teacherId = _userManager.GetUserId(User);

            var result = await _contentService.AddContentAsync(dto, teacherId);


            return Ok(result);
        }
    }
}
