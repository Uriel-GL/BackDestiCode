using AutoMapper;
using BackDestiCode.Data.Models;
using BackDestiCode.DTOs;
using BackDestiCode.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackDestiCode.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _serviceUsuario;
        private readonly IMapper _mapper;

        public AuthController(IAuthService serviceUsuario, IMapper mapper)
        {
            _serviceUsuario = serviceUsuario;
            _mapper = mapper;

        }

        [HttpPost("IniciarSesion")]
        public async Task<ActionResult<AuthResponse>> IniciarSesion([FromBody] AuthRequest request)
        {
            var response = await _serviceUsuario.Validar(request);

            return response.Resultado == true ? Ok(response) : BadRequest(response);
        }


        [HttpPost("RegistrarUsuario")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] AuthRegister registro)
        {
            try
            {
                var usuario = _mapper.Map<UsuariosDto, Usuarios>(registro.Usuario);
                var datosUsuario = _mapper.Map<DatosPersonalesDto, DatosPersonales>(registro.DatosPersonales);

                usuario.Id_Usuario = Guid.NewGuid();
                usuario.Fecha_Registro = DateTime.UtcNow;
                usuario.Estatus = true;

                datosUsuario.Id_DatosPersonales = Guid.NewGuid();
                datosUsuario.Id_Usuario = usuario.Id_Usuario;
                datosUsuario.Estatus = true;

                var response = await _serviceUsuario.Registrar(usuario, datosUsuario);

                if (response)
                {
                    return Ok(response);
                }

                return BadRequest("Ocurrio un error durante tu registro, intenta mas tarde");
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw new Exception("Ocurrio un error inesperado intenta mas tarde");
            }
        }
    }
}
