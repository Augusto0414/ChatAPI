using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatAPI.Models
{
    public class Mensaje
    {
        [BsonId]
        [BsonRepresentation (BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("senderID")]
        public string SenderId { get; set; }

        [BsonElement("reciverID")]
        public string ReceiverId { get; set; }


        [BsonElement("message")]
        public string Message { get; set; }


        [BsonElement ("fechaCreacion")]
        public DateTime FechaCreacion { get; set; }
    }
}
