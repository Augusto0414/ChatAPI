using ChatAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        private readonly IUsuario _usuario;

        public UsuarioController(IUsuario usuario)
        {
            _usuario = usuario;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarios = await _usuario.GetAllUserAsync();
            if (usuarios == null)
            {
                return NotFound("No se encontraron usuarios");
            }

            return Ok(new { usuarios });
        }

        [HttpGet("lasmessage")]
        public async Task<IActionResult> GetAllUserLasMessage()
        {
            var usuarios = await _usuario.GetAllUserLasMessage();
            if (usuarios == null)
            {
                return NotFound("No se encontraron usuarios");
            }

            return Ok(new { usuarios });
        }
    }
}
