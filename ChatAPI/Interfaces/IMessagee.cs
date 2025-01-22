using ChatAPI.Models;

namespace ChatAPI.Interfaces
{
    public interface IMessagee
    {
        Task<List<Mensaje>> GetMensajesAsync(string sender, string reciver);
        Task SaveMesaje(Mensaje mensaje); 
    }
}
