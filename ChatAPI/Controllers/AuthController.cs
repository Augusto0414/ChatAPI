using ChatAPI.Dtos;
using ChatAPI.Interfaces;
using ChatAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;
using System.Security.Claims;

namespace ChatAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUsuario _usuario;
        private readonly IAuth _authToken;

        public AuthController(IUsuario usuario, IAuth authToken)
        {
            _usuario = usuario;
            _authToken = authToken;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto) {
            var userExist = await _usuario.GetByEmailAsync(dto.Email);
            if (userExist != null) {
                return BadRequest("El usuario ya se encuentra registrado"); 
            }

            var user = new Usuario(
                name: dto.Username, 
                email: dto.Email,
                password: BCrypt.Net.BCrypt.HashPassword(dto.Password)
                );

            await _usuario.CreateUsuario(user);  

            return Ok(new { mesage =  "Usuario registrado correctamente" }); 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto) {

            var user = await _usuario.GetByEmailAsync(dto.Email);
            if (user == null ||  !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password)  ) {
                return BadRequest("Credenciales invalidas"); 
            }

            var token = _authToken.GenerateJwtToken(user);
            var userResponse = new
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
            };

            return Ok(new { token, userResponse }); 

        }

        [HttpGet("revalidate")]
        public IActionResult RevalidateToken()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { ok = false, error = "Token no válido" });
                }

                var principal = _authToken.ValidateJwtToken(token);
                if (principal == null)
                {
                    return Unauthorized(new { ok = false, error = "Token inválido o expirado" });
                }

                var name = principal.Identity.Name;
                var uid = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var newToken = _authToken.GenerateJwtToken(new Usuario { Name = name, Id = uid });

                var usuario = new
                {
                    ok = true,
                    uid,
                    name,
                    message = "Token renovado exitosamente"
                }; 

                return Ok(new { token = newToken, usuario });
            }
            catch
            {
                return StatusCode(500, new { ok = false, error = "Error al revalidar token" });
            }
        }

    }
}
