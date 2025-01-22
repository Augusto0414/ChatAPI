using ChatAPI.Validations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ChatAPI.Models
{
    public class Usuario
    {
        public Usuario(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }
        public Usuario() {
        
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] 
        public string Id { get; set; }
     
        [Required (ErrorMessage ="Nombre de usuario Requerido")]
        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }
        
        [Required (ErrorMessage ="Correo electronico es requerido")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        [BsonElement ("email")]
        public string Email { get; set; }
        
       [PasswordValidatorAttribute]
       [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("online")]
        [BsonDefaultValue(false)]
        public bool Online { get; set; } = false; 

    }
}
