using System;

namespace Public_Chat.Data
{
    public class NewMessageData
    {
        public Guid To { get; set; }
        public Guid Sender { get; set; }

        public DateTime Time { get; set; }
        public string Text { get; set; }
    }
}
