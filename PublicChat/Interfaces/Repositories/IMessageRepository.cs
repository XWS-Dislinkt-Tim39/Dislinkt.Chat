using Public_Chat.Data;
using Public_Chat.Domain;
using System;
using System.Threading.Tasks;

namespace Public_Chat.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        Task CreateChatAsync(MessageData chat);

        Task AddMessage(Message chat);
        Task<Message> GetBySenderAndReciever(Guid from, Guid to);
        Task<Message> GetById(Guid id);
    }
}
