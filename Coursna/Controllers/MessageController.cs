using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/v1/messages")]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetConversation(string userId)
        {
            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _messageService.GetConversationAsync(currentUser, userId);

            return Ok(result);
        }
    }
}
