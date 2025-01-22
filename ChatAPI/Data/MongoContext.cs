using ChatAPI.Models;
using MongoDB.Driver;

namespace ChatAPI.Data
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            _database = client.GetDatabase(config["MongoDB:DatabaseName"]);
        }
        public IMongoCollection<Usuario> Usuarios => _database.GetCollection<Usuario>("Usuarios");
        public IMongoCollection<Mensaje> Mensajes => _database.GetCollection<Mensaje>("Mensajes"); 
    }
}
