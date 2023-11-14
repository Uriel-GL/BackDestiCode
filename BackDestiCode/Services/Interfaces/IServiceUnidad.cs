using BackDestiCode.Data.Models;
using BackDestiCode.DTOs;

namespace BackDestiCode.Services.Interfaces
{
    public interface IServiceUnidad
    {
        Task<bool> Registrar(VehiculosDto vehiculos);

        Task<bool> Actualizar(Vehiculos vehiculo);

        Task<bool> Eliminar(Guid id_Unidad);

    }
}
