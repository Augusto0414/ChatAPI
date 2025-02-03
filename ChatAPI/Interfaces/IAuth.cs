using ChatAPI.Models;
using System.Security.Claims;

namespace ChatAPI.Interfaces
{
    public interface IAuth
    {
        string GenerateJwtToken(Usuario usuario);
        ClaimsPrincipal? ValidateJwtToken(string token);
    }
}
