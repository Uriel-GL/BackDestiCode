using BackDestiCode.DTOs;

namespace BackDestiCode.Services.Interfaces
{
    public interface IServiceUsuario
    {
        Task<List<DatosPersonalesDto>> GetUsuarios();
        Task<bool> InsertUsuario(DatosPersonalesDto usuario);
        Task<bool> UpdateUsuario(DatosPersonalesDto usuario);
    }
}
