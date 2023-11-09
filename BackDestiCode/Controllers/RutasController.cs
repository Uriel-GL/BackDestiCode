using BackDestiCode.DTOs;
using BackDestiCode.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackDestiCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RutasController : ControllerBase
    {
        private readonly IRutasService _rutasService;

        public RutasController(IRutasService rutaService)
        {
            _rutasService = rutaService;
        }
        [HttpPost]
        public async Task<IActionResult> RegistrarRuta([FromBody] RutasDto rutasDto)
        {
            try
            {
                if (rutasDto == null)
                {
                    return BadRequest("Los datos de la ruta son nulos.");
                }

                bool resultado = await _rutasService.Registrar(rutasDto);

                if (resultado)
                {
                    return CreatedAtAction("RegistrarRuta", new { id = rutasDto.Id_Ruta }, rutasDto);
                }
                else
                {
                    return StatusCode(500, "Error al registrar la ruta.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

    }
}
