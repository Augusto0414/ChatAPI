using ChatAPI.Interfaces;
using ChatAPI.Models;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

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
        public async Task ConnectUser(HttpContext context, WebSocket socket)
        {
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                _connectedUsers[userId] = socket;

                await ReceiveMessagesAsync(userId, socket);
            }
        }

        public async Task SendMessageToReceiverAsync(Mensaje message)
        {
            if (_connectedUsers.TryGetValue(message.ReceiverId, out var receiverSocket) &&
                receiverSocket.State == WebSocketState.Open)
            {
                var messageJson = System.Text.Json.JsonSerializer.Serialize(message);
                var messageBytes = Encoding.UTF8.GetBytes(messageJson);
                await receiverSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private async Task ReceiveMessagesAsync(string senderId, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _connectedUsers.Remove(senderId);
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Conecion cerrada", CancellationToken.None);
                }
            }
        }

    }
}

