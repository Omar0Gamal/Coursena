using Coursna.Core.Contracts;
using Coursna.Core.ServiceContracts;
using Coursna.Core.Dtos;
using Microsoft.AspNetCore.Mvc;


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

        [HttpGet("grades")]
        public async Task<IActionResult> GetGrades()
        {
            var result = await _lookupService.GetGradesAsync();
            return Ok(result);
        }
    }
}
