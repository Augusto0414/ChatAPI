﻿namespace ChatAPI.Dtos
{
    public class MessageDto
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
