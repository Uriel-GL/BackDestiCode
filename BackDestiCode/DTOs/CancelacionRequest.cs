namespace BackDestiCode.DTOs
{
    public class CancelacionRequest
    {
        public Guid Id_Ruta { get; set; }
        public Guid Id_Reservacion { get; set; }
        public int Num_Lugares_Cancelar { get; set; }
    }
}
