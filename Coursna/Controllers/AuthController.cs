using Coursna.Core.Contracts;
using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace Coursna.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILookUpService _lookupService;

        public AuthController(IAuthService authService, ILookUpService lookupService)
        {
            _authService = authService;
            _lookupService = lookupService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userForRegisterDto)
        {
            try 
            {
                var createdUser = await _authService.Register(userForRegisterDto);
                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userForLoginDto)
        {
            var response = await _authService.Login(userForLoginDto);

            if (!response.IsSuccess)
                return Unauthorized(response);

            return Ok(response);
        }
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> getMe()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var result = await _authService.GetMeAsync(userId);
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
