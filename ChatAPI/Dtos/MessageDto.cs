namespace ChatAPI.Dtos
{
    public class MessageDto
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
