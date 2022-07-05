using System;

namespace Public_Chat.Domain
{
    public class MessageInfo
    {
        public Guid Id { get; }
        public Guid Sender { get; }
        public string Text { get; }
        public DateTime Time { get; }

        public MessageInfo(Guid id, Guid sender, string text, DateTime time)
        {
            Id = id;
            Sender = sender;
            Text = text;
            Time = time;
        }
    }
}
