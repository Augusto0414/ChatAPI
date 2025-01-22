using ChatAPI.Models;

namespace ChatAPI.Interfaces
{
    public interface IAuth
    {
        string GenerateJwtToken(Usuario usuario); 
    }
}
