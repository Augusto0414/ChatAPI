namespace ChatAPI.Dtos
{
    public class MensajeUsuarioDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Online { get; set; }
        public string LastMessage { get; set; } 
    }
}
