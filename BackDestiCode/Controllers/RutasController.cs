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

        [HttpGet("GetRutasByUsuario/{Id_Usuario}")]
        public async Task<ActionResult<List<RutasDto>>> GetRutaByIdUsuario(Guid Id_Usuario)
        {
            return Ok(
                await _rutasService.GetRutasByIdUsuario(Id_Usuario)
                );
        }

        [HttpGet("GetRutasByIdRuta/{Id_Ruta}")]
        public async Task<ActionResult<RutasDto>> GetRutaByIdRuta(Guid Id_Ruta)
        {
            return Ok(
                await _rutasService.GetRutaByIdRuta(Id_Ruta)
                );
        }

        [HttpGet("GetAllRutas")]
        public async Task<ActionResult<List<RutasDto>>> GetAll()
        {
            return Ok(await _rutasService.GetAllRutas());
        }

        [HttpGet("GetTicketsByUser/{Id_Usuario}")]
        public async Task<ActionResult<List<RutasDto>>> GetTicketsByIdUsuario(Guid Id_Usuario)
        {
            return Ok(await _rutasService.GetTicketsReservacion(Id_Usuario));
        }

        [HttpGet("GetUsuariosReservaronRuta/{Id_Usuario}")]
        public async Task<ActionResult<List<UsuariosDto>>> GetUsuariosReservaronRuta(Guid Id_Usuario)
        {
            return Ok(await _rutasService.GetUsersByReservacion(Id_Usuario));
        }

        [HttpPost("RegistarRuta")]
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

        [HttpPost("ReservarLugar")]
        public async Task<IActionResult> ReservarLugar([FromBody] ReservacionRequest request)
        {
            try
            {
                var response = await _rutasService.ReservarLugar(request);

                return Ok(response);
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return BadRequest("Error al reservar un lugar en la ruta.");
            }
        }

        [HttpPost("CancelarLugar")]
        public async Task<IActionResult> CancelarLugar([FromBody] CancelacionRequest cancelacion)
        {
            try
            {
                var response = await _rutasService.CancelarReservacion(cancelacion);

                return response == true ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return BadRequest("Error al cancelar tu lugar en la ruta.");
            }
        }

        [HttpDelete("{id_Ruta}")]
        public async Task<IActionResult> EliminarRuta(Guid id_Ruta)
        {
            try
            {
                bool resultado = await _rutasService.EliminarManual(id_Ruta);

                if (resultado)
                {
                    return Ok("Ruta eliminada con éxito.");
                }
                else
                {
                    return NotFound("No se encontró la Ruta con el Id_Ruta proporcionado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }


    }
}
