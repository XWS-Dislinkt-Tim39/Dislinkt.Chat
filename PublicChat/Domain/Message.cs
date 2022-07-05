using System;

namespace Public_Chat.Domain
{
    public class Message
    {
        public Guid Id { get; }

        public Guid From { get; }

        public Guid To { get; }
        public MessageInfo[] Messages { get; }

        public Message(Guid id, Guid from, Guid to,MessageInfo[] messages)
        {
            Id = id;
           From = from;
            To = to;
            Messages = messages;
        }
    }
}
