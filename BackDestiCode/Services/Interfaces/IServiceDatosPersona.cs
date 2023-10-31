using BackDestiCode.DTOs;

namespace BackDestiCode.Services.Interfaces
{
    public interface IServiceDatosPersona
    {
        Task<List<DatosPersonalesDto>> GetUsuarios();
        Task<bool> InsertUsuario(DatosPersonalesDto usuario);
        Task<bool> UpdateUsuario(DatosPersonalesDto usuario);
        Task<bool> DeleteUsuario(Guid Id_Usuario);
    }
}
