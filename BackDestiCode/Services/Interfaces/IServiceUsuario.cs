using BackDestiCode.DTOs;

namespace BackDestiCode.Services.Interfaces
{
    public interface IServiceUsuario
    {
        Task<bool> Registrar(UsuariosDto usuario);
        Task<bool> Validar(string nombreUsuario, string contrasenia);
    }
}
