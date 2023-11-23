using BackDestiCode.Data.Models;
using BackDestiCode.DTOs;

namespace BackDestiCode.Services.Interfaces
{
    public interface IRutasService
    {
        Task<List<RutasDto>> GetAllRutas();
        Task<List<RutasDto>> GetRutasByIdUsuario(Guid Id_Usuario);
        Task<RutasDto> GetRutaByIdRuta(Guid Id_Ruta);
        Task<List<UsuariosDto>> GetUsersByReservacion(Guid Id_Ruta);
        Task<List<ReservacionesDto>> GetTicketsReservacion(Guid Id_Usuario);
        Task<bool> Registrar(RutasDto rutas);
        Task<bool> ReservarLugar(ReservacionRequest reservacion);
        Task<bool> CancelarReservacion(CancelacionRequest cancelacion);
        Task<bool> EliminarManual(Guid id_Ruta);
     
    }
}
