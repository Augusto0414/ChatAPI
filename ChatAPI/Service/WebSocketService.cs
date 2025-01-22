using ChatAPI.Interfaces;
using ChatAPI.Repositorio;
using System.Net.WebSockets;

namespace ChatAPI.Service
{
    public class WebSocketService
    {
        private readonly Dictionary<string, WebSocket> _connectedUsers = new();
        private readonly IMessagee _message;

        public WebSocketService(IMessagee message)
        {
            _message = message;
        }

    }
}
