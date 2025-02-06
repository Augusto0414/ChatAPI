using ChatAPI.Dtos;
using ChatAPI.Models;

namespace ChatAPI.Interfaces
{
    public interface IUsuario
    {

        Task<Usuario> GetByEmailAsync(string email);
        Task CreateUsuario(Usuario usuario);
        Task<List<Usuario>> GetAllUserAsync();
        Task<List<MensajeUsuarioDto>> GetAllUserLasMessage();
    }
}
