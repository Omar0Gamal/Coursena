using Coursna.Core.Dtos;
using Coursna.Core.Service;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/v1/teacher")]
    [Authorize(Roles = "Teacher")]
    public class TeacherController : ControllerBase
    {
        private readonly ICourseCodeService _codeService;

        public TeacherController(ICourseCodeService codeService)
        {
            _codeService = codeService;
        }

        [HttpPost("generate-codes")]
        public async Task<IActionResult> GenerateCodes(GenerateCodeDto dto)
        {
            var result = await _codeService.GenerateCodesAsync(dto.CourseId, dto.Count);


            return Ok(result);
        }
        [HttpGet("courses/{courseId}/invite-codes")]
        public async Task<IActionResult> GetCodes(int courseId)
        {
           
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);

   
            var result = await _codeService.GetCodesAsync(teacherId, courseId);

            return Ok(result);
        }
  
    }

}
