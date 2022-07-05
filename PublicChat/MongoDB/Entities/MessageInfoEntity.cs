using Public_Chat.Domain;
using System;
using System.Linq;

namespace Public_Chat.MongoDB.Entities
{
    public class MessageInfoEntity : BaseEntity
    {


        public Guid Sender { get; set; }

        public string Text { get; set; }
     

        public static MessageInfoEntity ToMessageInfoEntity(MessageInfo messageInfo)
        {
            return new MessageInfoEntity
            {
                Id = messageInfo.Id,
                Sender = messageInfo.Sender,
                Text = messageInfo.Text,

            };
        }

        public static MessageInfoEntity[] ToMessageInfoEntities(MessageInfo[] messages)
          => messages.Select(p => ToMessageInfoEntity(p)).ToArray();

        public MessageInfo ToMessageinfo()
            => new MessageInfo(this.Id, this.Sender, this.Text);

    }
}
