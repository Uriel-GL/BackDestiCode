using AutoMapper;
using BackDestiCode.Data.Context;
using BackDestiCode.Data.Models;
using BackDestiCode.DTOs;
using BackDestiCode.Services.Interfaces;

namespace BackDestiCode.Services.Repository
{
    public class ServiceUnidad : IServiceUnidad
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        public ServiceUnidad(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Actualizar(Vehiculos vehiculo)
        {
            bool respuesta = false;
            try
            {
                var unidad = _context.Vehiculos.Where(id => id.Id_Unidad.Equals(vehiculo.Id_Unidad)).FirstOrDefault();

                if (unidad != null)
                {
                    unidad.Color = vehiculo.Color;
                    unidad.Placa = vehiculo.Placa;
                    unidad.Imagen = vehiculo.Imagen;
                    unidad.Modelo = vehiculo.Modelo;

                    await _context.SaveChangesAsync();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return respuesta;
        }

        public async Task<bool> Registrar(VehiculosDto vehiculos)
        {
            var respuesta = false;
            if (vehiculos != null)
            {
                try
                {

                    var vehiculosDto = new VehiculosDto
                    {

                        Id_Unidad = vehiculos.Id_Unidad,
                        Id_Usuario = vehiculos.Id_Usuario,
                        Color = vehiculos.Color,
                        Placa = vehiculos.Placa,
                        Imagen = vehiculos.Imagen,
                        Modelo = vehiculos.Modelo,

                    };
                    var unidadNueva = _mapper.Map<VehiculosDto, Vehiculos>(vehiculosDto);
                    await _context.Vehiculos.AddAsync(unidadNueva);
                    await _context.SaveChangesAsync();
                    respuesta = true;
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    respuesta = false;
                }
            }
            else { respuesta = false; }
            return respuesta;
        }

        public async Task<bool> Eliminar(Guid id_Unidad)
        {
            try
            {
                var unidadAEliminar = await _context.Vehiculos.FindAsync(id_Unidad);

                if (unidadAEliminar == null)
                {
                    return false; // La unidad no existe
                }

                _context.Vehiculos.Remove(unidadAEliminar);
                await _context.SaveChangesAsync();
                return true; // Éxito al eliminar la unidad
            }
            catch (Exception ex)
            {
                // Registra la excepción para depuración
                var msg = ex.Message;
                return false; // Error al intentar eliminar la unidad
            }
        }


    }
}
