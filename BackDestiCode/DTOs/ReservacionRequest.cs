namespace BackDestiCode.DTOs
{
    public class ReservacionRequest
    {
        public Guid Id_Reservacion { get; set; }
        public Guid Id_Ruta { get; set; }
        public Guid Id_Usuario { get; set; }
        public int Num_Asientos { get; set; }
    }
}
