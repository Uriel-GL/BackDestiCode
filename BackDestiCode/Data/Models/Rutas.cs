namespace BackDestiCode.Data.Models
{
    public class Rutas
    {
        //El Id _ruta se modifico a guid
        public Guid Id_Ruta { get; set; }
        public Guid Id_Unidad { get; set; }
        public Guid Id_Usuario { get; set; }
        public string Lugar_Salida { get; set; }
        public string Lugar_Destino { get; set; }
        public DateTime Fecha_Salidad { get; set; }
        public decimal Costo { get; set; }
        public int Lugares_Disponibles { get; set; }
        public bool Estatus { get; set; }

        //Relaciones
        public Usuarios Usuarios { get; set; }
        public Vehiculos Vehiculos { get; set; }
        public ICollection<Reservaciones>? Reservaciones { get; set; }
    }
}
