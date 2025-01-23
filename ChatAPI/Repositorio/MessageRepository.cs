using ChatAPI.Data;
using ChatAPI.Interfaces;
using ChatAPI.Models;
using MongoDB.Driver;
using MongoDB.Driver.Core.Servers;

namespace ChatAPI.Repositorio
{
    public class MessageRepository : IMessagee
    {
        private readonly IMongoCollection<Mensaje> _messages;

        public MessageRepository(MongoContext mongoContext)
        {
            _messages = mongoContext.Mensajes;
        }

        public async Task<List<Mensaje>> GetMensajesAsync(string sender, string receiver)
        {
            var filter  = Builders<Mensaje>.Filter.And(
            Builders<Mensaje>.Filter.Or(
                    Builders<Mensaje>.Filter.Eq(m => m.SenderId, sender),
                    Builders<Mensaje>.Filter.Eq(m => m.ReceiverId, sender)
                ),
                Builders<Mensaje>.Filter.Or(
                    Builders<Mensaje>.Filter.Eq(m => m.SenderId, receiver),
                    Builders<Mensaje>.Filter.Eq(m => m.ReceiverId, receiver)
                )
            );


            return await _messages.Find(filter).ToListAsync();  
        }

        public async Task SaveMesaje(Mensaje mensaje)
        {
            await _messages.InsertOneAsync(mensaje);
        }
    }
}
