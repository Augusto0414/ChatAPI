using ChatAPI.Interfaces;
using ChatAPI.Models;
using System.Net.WebSockets;
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
        public async Task ConnectUser(string userName, WebSocket socket) 
        {
            _connectedUsers[userName] = socket;
            await ReceiveMessagesAsync(userName ,socket);
        }

        private async Task ReceiveMessagesAsync(string sender, WebSocket webSocket) {

            var buffer = new byte[1024 * 4];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _connectedUsers.Remove(sender);
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Conexión cerrada", CancellationToken.None);
                    return; 
                }
                    var messageJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var message = System.Text.Json.JsonSerializer.Deserialize<Mensaje>(messageJson);

                    if (_connectedUsers.TryGetValue(message.ReceiverId, out var receiverSocket) &&
                        receiverSocket.State == WebSocketState.Open)
                    {
                        // Forward the message to the receiver
                        var forwardMessage = Encoding.UTF8.GetBytes(messageJson);
                        await receiverSocket.SendAsync(new ArraySegment<byte>(forwardMessage), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    else
                    {
                        // Save the message to the database if the receiver is not online
                        await _message.SaveMesaje(message);
                    }
                
            }
        }
    }
 }

