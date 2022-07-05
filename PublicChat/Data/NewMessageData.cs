using System;

namespace Public_Chat.Data
{
    public class NewMessageData
    {
        public Guid ChatId { get; set; }
        public Guid Sender { get; set; }

        public string Text { get; set; }
    }
}
