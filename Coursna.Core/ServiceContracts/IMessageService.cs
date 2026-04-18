using Coursna.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.ServiceContracts
{
    public interface IMessageService
    {
        Task<AuthResponseDto> SendMessageAsync(string senderId, SendMessageDto dto);
        Task<List<MessageResponseDto>> GetConversationAsync(string currentUserId, string otherUserId);
    }
}
