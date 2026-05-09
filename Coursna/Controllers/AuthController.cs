using Coursna.Core.Contracts;
using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coursna.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILookUpService _lookupService;
        public AuthController(IAuthService authService,ILookUpService lookUpService)
        {
            _authService = authService;
            _lookupService = lookUpService;
        }
        [HttpPost("register/teacher")]
        public async Task<IActionResult> ResgisterTeacher(RegisterTeacherDto dto)
        {
            var result = await _authService.RegisterTeacherAsync(dto);

            return Ok(result);
        }
        [HttpPost("register/student")]
        public async Task<IActionResult> RegisterStudent(RegisterStudentDto dto)
        {
            var result = await _authService.RegisterStudentAsync(dto);
   
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            return Ok(result);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
           var result= await _authService.LogoutAsync();
            return Ok(result);
        }
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> Update(RegisterTeacherDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _authService.Update(userId, dto);


            return Ok(result);
        }
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _authService.GetCurrentUserAsync(userId);

            return Ok(result);
        }

        [HttpGet("grades")]
        public async Task<IActionResult> GetGrades()
        {
            var result = await _lookupService.GetGradesAsync();
            return Ok(result);
        }
    }
}
