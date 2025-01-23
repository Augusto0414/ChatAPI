using ChatAPI.Interfaces;
using ChatAPI.Models;
using ChatAPI.Repositorio;
using ChatAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IMessagee _messagee;
        private readonly WebSocketService _socketService;

        public ChatController(IMessagee messagee, WebSocketService socketService)
        {
            _messagee = messagee;
            _socketService = socketService;
        }

        [HttpGet("receiver")]
        public async Task<IActionResult> GetMensaje (string receiverID) {

            var sender = User.FindFirstValue(ClaimTypes.Name); 
            var messages = await _messagee.GetMensajesAsync(sender, receiverID);
            return Ok(messages);

        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] Mensaje message)
        {
            message.FechaCreacion = DateTime.UtcNow;
            await _messagee.SaveMesaje(message);
            await _socketService.SendMessageToReceiverAsync(message);
            return Ok();
        }
    }
}
