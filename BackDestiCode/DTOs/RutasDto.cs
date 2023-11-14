namespace BackDestiCode.DTOs
{
    public class RutasDto
    {
        public Guid Id_Ruta { get; set; }
        public Guid Id_Unidad { get; set; }
        public Guid Id_Usuario { get; set; }
        public string Lugar_Salida { get; set; }
        public string Lugar_Destino { get; set; }
        public DateTime Fecha_Salida { get; set; }
        public double Costo { get; set; }
        public int Lugares_Disponibles { get; set; }
        public bool Estatus { get; set; }

    }
}
