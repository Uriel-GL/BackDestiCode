using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackDestiCode.Data.Models
{
    public class DatosPersonales
    {
        public Guid Id_DatosPersonales { get; set; }
        public Guid Id_Usuario { get; set; }
        public string Nombre_Completo { get; set; }
        public DateTime Fecha_Nacimiento { get; set; }
        public string Matricula { get; set; }
        public string Grupo { get; set; }
        public string Credencial { get; set; }
        public string Universidad { get; set; }
        public Int64 Telefono { get; set; }
        public string Correo { get; set; }
        public bool Estatus { get; set; }

        //Relaciones
        public Usuarios Usuarios { get; set; }
    }
}
