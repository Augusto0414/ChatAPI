using ChatAPI.Data;
using ChatAPI.Interfaces;
using ChatAPI.Models;
using MongoDB.Driver;

namespace ChatAPI.Repositorio
{
    public class MessageRepository : IMessagee
    {
        private readonly MongoContext _mongoContext;

        public MessageRepository(MongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task<List<Mensaje>> GetMensajesAsync(string sender, string reciver)
        {
            return await _mongoContext.Mensajes.Find( message => (message.SenderId == sender && message.ReceiverId == reciver)
            || (message.SenderId == reciver && message.ReceiverId == sender)).ToListAsync();
        }

        public async Task SaveMesaje(Mensaje mensaje)
        {
            await _mongoContext.Mensajes.InsertOneAsync(mensaje);
        }
    }
}
