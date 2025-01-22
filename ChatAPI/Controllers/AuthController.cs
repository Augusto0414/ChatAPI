using ChatAPI.Dtos;
using ChatAPI.Interfaces;
using ChatAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;

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
            var userExist = _usuario.GetByEmailAsync(dto.Email);
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

            return Ok(new { token }); 

        }
    }
}
