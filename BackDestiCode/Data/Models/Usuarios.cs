using System.ComponentModel.DataAnnotations;

namespace BackDestiCode.Data.Models
{
    public class Usuarios
    {
        public Guid Id_Usuario { get; set; }
        public string Nombre_Usuario { get; set; }
        public string Correo { get; set; }
        public string Contrasenia { get; set;}
        public string? Token { get; set;}
        public DateTime Fecha_Registro { get; set; }
        public bool Estatus { get; set; }

        public ICollection<DatosPersonales> DatosPersonales { get; set; }
    }
}
