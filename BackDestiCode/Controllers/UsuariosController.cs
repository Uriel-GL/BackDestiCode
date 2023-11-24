using BackDestiCode.DTOs;
using BackDestiCode.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackDestiCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("GetUsuarioInfo/{Id_Usuario}")]
        public async Task<ActionResult<UsuariosDto>> GetUsuarioById(Guid Id_Usuario)
        {
            return Ok(await _usuarioService.GetUsuarioById(Id_Usuario));
        }

        [HttpPost("ActualizarDatosPersonales")]
        public async Task<IActionResult> UpdateUsuarioInfo([FromBody] AuthRegister authUpdate)
        {
            return Ok(await _usuarioService.UpdateUsuario(authUpdate));
        }

    }
}
