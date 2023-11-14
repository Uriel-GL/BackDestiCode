using BackDestiCode.Data.Models;
using BackDestiCode.DTOs;

namespace BackDestiCode.Services.Interfaces
{
    public interface IRutasService
    {
        Task<bool> Registrar(RutasDto rutas);
        Task<bool> ReservarLugar(ReservacionRequest reservacion);
        Task<bool> CancelarReservacion(CancelacionRequest cancelacion);
        Task<bool> EliminarManual(Guid id_Ruta);


    }
}
