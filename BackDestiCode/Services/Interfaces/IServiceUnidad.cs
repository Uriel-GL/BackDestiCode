using BackDestiCode.Data.Models;
using BackDestiCode.DTOs;

namespace BackDestiCode.Services.Interfaces
{
    public interface IServiceUnidad
    {
        Task<List<VehiculosDto>> GetVehiculosByUsuario(Guid Id_Usuario);
        Task<bool> Registrar(VehiculosDto vehiculos);

        Task<bool> Actualizar(Vehiculos vehiculo);

        Task<bool> Eliminar(Guid id_Unidad);

    }
}
