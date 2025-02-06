using ChatAPI.Data;
using ChatAPI.Dtos;
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

        public async Task<List<Usuario>> GetAllUserAsync()
        {
           // var usuarios = Builders<Usuario>.Filter.Empty;
            return await _mongoContext.Usuarios.Find(_ => true).ToListAsync();
        }

        public async Task<List<MensajeUsuarioDto>> GetAllUserLasMessage()
        {
          var usuarios = await _mongoContext.Usuarios.Find(_ => true).ToListAsync();

          var usuariosConUltimoMensaje = new List<MensajeUsuarioDto>();


            foreach (var usuario in usuarios)
            {
                var ultimoMensaje = await _mongoContext.Mensajes
                .Find(m => m.SenderId == usuario.Id)
                .SortByDescending(m => m.FechaCreacion)
                .Limit(1)
                .FirstOrDefaultAsync();

                // Crear un objeto que combine el usuario y su último mensaje
                var usuarioConMensaje = new MensajeUsuarioDto
                {
                    Id = usuario.Id,
                    Name = usuario.Name,
                    Email = usuario.Email,
                    Password = usuario.Password,
                    Online = usuario.Online,
                    LastMessage = ultimoMensaje?.Message 
                };

                usuariosConUltimoMensaje.Add(usuarioConMensaje);

            }
            return usuariosConUltimoMensaje;

        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _mongoContext.Usuarios.Find( user => user.Email == email).FirstOrDefaultAsync();
        }
    }
}
