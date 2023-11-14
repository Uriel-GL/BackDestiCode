using BackDestiCode.Data.Models;
using BackDestiCode.DTOs;

namespace BackDestiCode.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Registrar(Usuarios usuario, DatosPersonales datosUsuario);
        Task<AuthResponse> Validar(AuthRequest request);
        Task<bool> RecuperarContrasenia(RecuperarContraseniaRequest request);
        Task<bool> EnviarCorreo(CorreoRequest request);
        Task<bool> VerificarTokenRecuperacion(RecuperarContraseniaRequest request);
        Task<bool> ResetearContrasenia(RecuperarContraseniaRequest request);
    }
}
