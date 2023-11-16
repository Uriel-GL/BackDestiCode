namespace BackDestiCode.DTOs
{
    public class RecuperarContraseniaRequest
    {
        public string Correo { get; set; }
        public string Contrasenia { get; set; }
        public string? Token { get; set; }
    }
}
