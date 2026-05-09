using Coursna.Core.Domain.Entities;
using Coursna.Core.Domain.RepositoryInterface;
using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using Coursna.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Service
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepo;

        public MessageService(IMessageRepository messageRepo)
        {
            _messageRepo = messageRepo;
        }
        public async Task<List<MessageResponseDto>> GetConversationAsync(string currentUserId, string otherUserId)
        {
            var messages = await _messageRepo.GetConversationAsync(currentUserId, otherUserId);

            return messages
                .Select(m => m.ToResponse())
                .ToList();
        }

        public async Task<ApiResponseDto> SendMessageAsync(string senderId, SendMessageDto dto)
        {
            if (string.IsNullOrEmpty(dto.Content))
            {
                throw new BadRequestException("Message content cannot be empty");
            }
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = dto.ReceiverId,
                content = dto.Content,
                SentAt = DateTime.UtcNow
            };
            await _messageRepo.AddAsync(message);
            return ApiResponseDto.Success("Message sent");
        }
    }
}
