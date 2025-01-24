using ChatAPI.Models;
using MongoDB.Driver;

namespace ChatAPI.Data
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext(IConfiguration config)
        {
            var connectionString = config["MongoDB:ConnectionString"];
            var databaseName = config["MongoDB:DatabaseName"];

            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(databaseName))
            {
                throw new InvalidOperationException("MongoDB configuration is missing or invalid.");
            }

            try
            {
                var client = new MongoClient(connectionString);
                _database = client.GetDatabase(databaseName);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not connect to MongoDB: {ex.Message}", ex);
            }
        }


        public IMongoCollection<Usuario> Usuarios => _database.GetCollection<Usuario>("Usuarios");
        public IMongoCollection<Mensaje> Mensajes => _database.GetCollection<Mensaje>("Mensajes"); 
    }
}
