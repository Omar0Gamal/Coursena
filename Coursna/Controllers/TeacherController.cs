using Coursna.Core.Dtos;
using Coursna.Core.Contracts;
using Coursna.Core.Service;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IAuthService _authService;

        public TeacherController(ICourseCodeService codeService, IAuthService authService)
        {
            _codeService = codeService;
            _authService = authService;
        }

        [HttpGet("my-students")]
        public async Task<IActionResult> GetMyStudents()
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _authService.GetMyStudentsAsync(teacherId);
            return Ok(result);
        }

        [HttpPost("generate-codes")]
        public async Task<IActionResult> GenerateCodes(GenerateCodeDto dto)
        {
            var result = await _codeService.GenerateCodesAsync(dto.CourseId, dto.Count);


            return Ok(result);
        }
        [HttpGet("courses/{courseId}/enroll-codes")]
        public async Task<IActionResult> GetCodes(int courseId)
        {
           
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);

   
            var result = await _codeService.GetCodesAsync(teacherId, courseId);

            return Ok(result);
        }

        [HttpGet("courses/{courseId}/active-codes")]
        public async Task<IActionResult> GetActiveCodes(int courseId)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _codeService.GetActiveCodesAsync(teacherId, courseId);

            return Ok(result);
        }
  
    }

}
