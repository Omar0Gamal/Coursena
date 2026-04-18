using Coursna.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class MessageResponseDto
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
    public static class MessageExtention
    {
        public static MessageResponseDto ToResponse(this Message m)
        {
            return new MessageResponseDto
            {
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.content,
                SentAt = m.SentAt
            };
        }
    }
}
