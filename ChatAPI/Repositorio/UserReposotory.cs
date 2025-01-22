using ChatAPI.Data;
using ChatAPI.Interfaces;
using ChatAPI.Models;
using MongoDB.Driver;

namespace ChatAPI.Repositorio
{
    public class UserReposotory : IUsuario
    {
        private readonly MongoContext _mongoContext;

        public UserReposotory(MongoContext mongoContext)
        {
            this._mongoContext = mongoContext;
        }

       public async Task CreateUsuario(Usuario usuario)
        {
            await _mongoContext.Usuarios.InsertOneAsync(usuario); 
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _mongoContext.Usuarios.Find( user => user.Email == email).FirstOrDefaultAsync();
        }
    }
}
