namespace Email.BL
{
    public class Message : IMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }

    }
}
