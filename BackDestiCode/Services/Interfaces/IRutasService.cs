using BackDestiCode.DTOs;

namespace BackDestiCode.Services.Interfaces
{
    public interface IRutasService
    {
        Task<bool> Registrar(RutasDto rutas);

    }
}
