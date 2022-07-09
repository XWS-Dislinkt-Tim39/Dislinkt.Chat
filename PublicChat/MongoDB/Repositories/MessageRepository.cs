using Microsoft.VisualBasic;
using MongoDB.Driver;
using Public_Chat.Data;
using Public_Chat.Domain;
using Public_Chat.Interfaces.Repositories;
using Public_Chat.MongoDB.Common;
using Public_Chat.MongoDB.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Public_Chat.MongoDB.Repositories
{
    public class MessageRepository :IMessageRepository
    {
        private readonly IQueryExecutor _queryExecutor;
        public MessageRepository(IQueryExecutor queryExecutor)
        {
            _queryExecutor = queryExecutor;
        }
        public async Task CreateChatAsync(MessageData chat)
        {
            try
            {
                await _queryExecutor.CreateAsync(MessageEntity.ToMessageEntity(new Message(Guid.NewGuid(), chat.From,chat.To, Array.Empty<MessageInfo>())));
            }
            catch (MongoWriteException ex)
            {
                throw ex;
            }

        }

        public async Task<Message> GetById(Guid id)
        {
            var result = await _queryExecutor.FindByIdAsync<MessageEntity>(id);

            return result?.ToMessage() ?? null;
        }

   

        public async Task<Message> GetBySenderAndReciever(Guid from, Guid to)
        {
            var filter1 = Builders<MessageEntity>.Filter.Eq(u => u.From, from)
                & Builders<MessageEntity>.Filter.Eq(u => u.To, to);
            var filter2= Builders<MessageEntity>.Filter.Eq(u => u.From, to)
                & Builders<MessageEntity>.Filter.Eq(u => u.To, from);

            var result1 = await _queryExecutor.FindAsync(filter1);
            var result2= await _queryExecutor.FindAsync(filter2);

            var r1= result1?.AsEnumerable()?.FirstOrDefault(u => u.From == from)?.ToMessage() ?? null;
            var r2 = result2?.AsEnumerable()?.FirstOrDefault(u => u.From == to)?.ToMessage() ?? null;

            if (r1 != null)
            {
                return r1;
            }
            else
            {
                return r2;
            }
            
        }

        public async Task AddMessage(Message chat)
        {
            var filter = Builders<MessageEntity>.Filter.Eq(u => u.Id, chat.Id);

            var update = Builders<MessageEntity>.Update.Set(u => u.Messages, MessageInfoEntity.ToMessageInfoEntities(chat.Messages));

            await _queryExecutor.UpdateAsync(filter, update);
        }
        public async Task DeleteChat(Guid from, Guid to)
        {
            var filter1 = Builders<MessageEntity>.Filter.Eq(u => u.From, from)
                & Builders<MessageEntity>.Filter.Eq(u => u.To, to);
            var filter2 = Builders<MessageEntity>.Filter.Eq(u => u.From, to)
                & Builders<MessageEntity>.Filter.Eq(u => u.To, from);

            var result1 = await _queryExecutor.FindAsync(filter1);
            var result2 = await _queryExecutor.FindAsync(filter2);

            var r1 = result1?.AsEnumerable()?.FirstOrDefault(u => u.From == from)?.ToMessage() ?? null;
            var r2 = result2?.AsEnumerable()?.FirstOrDefault(u => u.From == to)?.ToMessage() ?? null;

            if (r1 != null)
            {
                await _queryExecutor.DeleteByIdAsync(filter1);
            }
            else if(r2!=null)
            {
                await _queryExecutor.DeleteByIdAsync(filter2);
            }
        }

      
        private MessageEntity ToMessageEntity(Message chat)
        {
            return new MessageEntity
            {
                Id = chat.Id,
                From = chat.From,
                To = chat.To,
              
            };
        }
    }
}
