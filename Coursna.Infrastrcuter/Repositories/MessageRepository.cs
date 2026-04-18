using Coursna.Core.Domain.Entities;
using Coursna.Core.Domain.RepositoryInterface;
using Coursna.Infrastrcuter.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Infrastrcuter.Repositories
{
    public class MessageRepository: Repository<Message>, IMessageRepository
    {
        private readonly AppDbContext _appDbContext;
        public MessageRepository(AppDbContext appDbContext):base(appDbContext) 
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Message>> GetConversationAsync(string user1, string user2)
        {
            return await _appDbContext.Messages.Where(m =>
                (m.SenderId == user1 && m.ReceiverId == user2) ||
                (m.SenderId == user2 && m.ReceiverId == user1))
            .OrderBy(m => m.SentAt)
            .ToListAsync();
        }
    }
}
