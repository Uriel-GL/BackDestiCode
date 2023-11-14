using BackDestiCode.DTOs;
using BackDestiCode.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackDestiCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculosController : ControllerBase
    {
        private readonly IServiceUnidad _unidadService;

        public VehiculosController(IServiceUnidad unidadService)
        {
            _unidadService = unidadService;
        }
        [HttpPost]
        public async Task<IActionResult> RegistrarUnidad([FromBody] VehiculosDto vehiculosDto)
        {
            try
            {
                if (vehiculosDto == null)
                {
                    return BadRequest("Los datos de la unidad son nulos.");
                }

                bool resultado = await _unidadService.Registrar(vehiculosDto);

                if (resultado)
                {
                    return CreatedAtAction("RegistrarUnidad", new { id = vehiculosDto.Id_Unidad }, vehiculosDto);
                }
                else
                {
                    return StatusCode(500, "Error al registrar la unidad.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }


        [HttpDelete("{id_Unidad}")]
        public async Task<IActionResult> EliminarUnidad(Guid id_Unidad)
        {
            try
            {
                bool resultado = await _unidadService.Eliminar(id_Unidad);

                if (resultado)
                {
                    return Ok("Unidad eliminada con éxito.");
                }
                else
                {
                    return NotFound("No se encontró la unidad con el Id_Unidad proporcionado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }
    }

}



