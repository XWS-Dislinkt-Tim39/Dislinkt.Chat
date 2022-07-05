using Public_Chat.Domain;
using Public_Chat.MongoDB.Attributes;
using System;
using System.Linq;

namespace Public_Chat.MongoDB.Entities
{
    [CollectionName("Chats")]
    public class MessageEntity:BaseEntity
    {

        public Guid From { get; set; }

        public Guid To { get; set; }
        public MessageInfoEntity[] Messages { get; set; }

        public static MessageEntity ToMessageEntity(Message message)
        {
            return new MessageEntity
            {
                Id = message.Id,
                From = message.From,
                To = message.To,
                Messages= MessageInfoEntity.ToMessageInfoEntities(message.Messages),
               
            };
        }
        public Message ToMessage()
            => new Message(this.Id, this.From, this.To, this.Messages == null ? Array.Empty<MessageInfo>() : this.Messages.Select(p => p.ToMessageinfo()).ToArray());

    }
}
