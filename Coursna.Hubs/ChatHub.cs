using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Coursna.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;


        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }



        // Lma el user by3mel connect m3 el hub 
        public override async Task OnConnectedAsync()
        {
            // hangyeb el user 
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (!string.IsNullOrEmpty(userId))
            {
                // bn3mel group ll user 34an n3rf n send message
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }


            await base.OnConnectedAsync();
        }
        //dy el send message f3lan
        public async Task SendMessage(string receiverId, string content)
        {
            // bngeb el senderId
            var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var senderName = Context.User?.FindFirst(ClaimTypes.Name)?.Value;

          
            if (string.IsNullOrEmpty(senderId))
                throw new Exception("Unauthorized");

           //save message in db 
            await _messageService.SendMessageAsync(senderId, new SendMessageDto
            {
                ReceiverId = receiverId,
                Content = content
            });

            // hna send el message to reciver k real-time
            await Clients.Group(receiverId).SendAsync("ReceiveMessage", new
            {
                SenderId = senderId,
                SenderName = senderName,
                Content = content,
                SentAt = DateTime.UtcNow
            });

            // send it also to sender 34an tabn 3ando bardo
            await Clients.Group(senderId).SendAsync("ReceiveMessage", new
            {
                SenderId = senderId,
                SenderName = senderName,
                Content = content,
                SentAt = DateTime.UtcNow
            });

        }
    }
}
