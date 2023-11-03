namespace BackDestiCode.Data.Models
{
    public class Rutas
    {
        public int Id_Ruta { get; set; }
        public int Id_Unidad { get; set; }
        public int Id_Usuario { get; set; }
        public int Lugar_Salida { get; set; }
        public int Lugar_Destino { get; set; }
        public int Fecha_Salida { get; set; }
        public int Costo { get; set; }
        public int Lugares_Dispinibles { get; set; }
        public int Estatus { get; set; }

        //Relaciones
        public Usuarios Usuarios { get; set; }
        public Vehiculos Vehiculos { get; set; }
        public ICollection<Reservaciones>? Reservaciones { get; set; }
    }
}
