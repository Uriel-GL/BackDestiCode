using BackDestiCode.DTOs;

namespace BackDestiCode.Services.Interfaces
{
    public interface IServiceUnidad
    {
        Task<bool> Registrar(VehiculosDto vehiculos);

    }
}
