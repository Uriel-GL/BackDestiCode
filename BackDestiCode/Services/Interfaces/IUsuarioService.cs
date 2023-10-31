using BackDestiCode.DTOs;

namespace BackDestiCode.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<bool> UpdateUsuario(AuthRegister datos);
        Task<bool> DeleteUsuario(Guid Id_Usuario);
    }
}
