using Coursna.Core.Contracts;
using Coursna.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coursna.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register-teacher")]
        public async Task<IActionResult> ResgisterTeacher(RegisterTeacherDto dto)
        {
            var result = await _authService.RegisterTeacherAsync(dto);
            if (!result.IsSuccess)
            {
                return Problem(
                    title: "Registration Failed",
                    detail: result.Message,
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
            return Ok(result);
        }
        [HttpPost("register-student")]
        public async Task<IActionResult> RegisterStudent(RegisterStudentDto dto)
        {
            var result = await _authService.RegisterStudentAsync(dto);
            if (!result.IsSuccess)
            {
                return Problem(
                    title: "Registration Failed",
                    detail: result.Message,
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.IsSuccess)
            {
                return Problem(
                    title: "Authentication Failed",
                    detail: result.Message,
                    statusCode: StatusCodes.Status400BadRequest
                );
            }
            return Ok(result);
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
           var result= await _authService.LogoutAsync();
            return Ok(result);
        }
        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> Update(RegisterTeacherDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _authService.Update(userId, dto);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
