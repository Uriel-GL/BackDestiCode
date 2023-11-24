using AutoMapper;
using BackDestiCode.Data.Models;
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
        private readonly IMapper _mapper;

        public VehiculosController(IServiceUnidad unidadService, IMapper mapper)
        {
            _unidadService = unidadService;
            _mapper = mapper;
        }

        [HttpPost("ActualizarVehiculo")]
        public async Task<ActionResult> ActualizarVehiculo([FromBody] VehiculosDto vehiculos)
        {
            var data = _mapper.Map<Vehiculos>(vehiculos);
            return Ok(await _unidadService.Actualizar(data));
        }

        [HttpPost("RegistrarVehiculo")]
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

        [HttpDelete("EliminarVehiculo/{Id_Unidad}")]
        public async Task<IActionResult> EliminarUnidad(Guid Id_Unidad)
        {
            try
            {
                bool resultado = await _unidadService.Eliminar(Id_Unidad);

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

        [HttpGet("GetVehiculosByUsuario/{Id_Usuario}")]
        public async Task<ActionResult<List<VehiculosDto>>> GetVehiculosByIdUsuario(Guid Id_Usuario)
        {
            return Ok(await _unidadService.GetVehiculosByUsuario(Id_Usuario));
        }

        [HttpGet("VehiculoById/{Id_Unidad}")]
        public async Task<ActionResult<VehiculosDto>> GetVehiculoById(Guid Id_Unidad)
        {
            return Ok(await _unidadService.GetVehiculoById(Id_Unidad));
        }
    }

}



