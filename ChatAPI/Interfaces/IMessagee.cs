using ChatAPI.Dtos;
using ChatAPI.Models;

namespace ChatAPI.Interfaces
{
    public interface IMessagee
    {
        Task<List<Mensaje>> GetMensajesAsync(string sender, string reciver);
        Task SaveMesaje(MessageDto mensaje); 
    }
}
