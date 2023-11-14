using AutoMapper;
using BackDestiCode.Data.Context;
using BackDestiCode.Data.Models;
using BackDestiCode.DTOs;
using BackDestiCode.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackDestiCode.Services.Repository
{
    public class RutasService : IRutasService

    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        public RutasService(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CancelarReservacion(CancelacionRequest cancelacion)
        {
            var response = false;
            try
            {
                _context.Database.BeginTransaction();

                var ruta = _context.Rutas.Where(r => r.Id_Ruta.Equals(cancelacion.Id_Ruta)).FirstOrDefault();
                var reservacion = _context.Reservaciones.Where(r => r.Id_Reservacion.Equals(cancelacion.Id_Reservacion)).FirstOrDefault();

                if (ruta is null) 
                    throw new Exception("La ruta con el Id especificado no existe.");

                if (reservacion is null)
                    throw new Exception("La reservacion con el Id especificado no existe.");

                if(ruta.Lugares_Disponibles != 0 && ruta.Estatus != false)
                {
                    ruta.Lugares_Disponibles += cancelacion.Num_Lugares_Cancelar;
                }
                else
                {
                    ruta.Lugares_Disponibles += cancelacion.Num_Lugares_Cancelar;
                    ruta.Estatus = true;
                }

                _context.Reservaciones.Remove(reservacion);

                await _context.SaveChangesAsync();
                _context.Database.CommitTransaction();

                return response = true;
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                string msg = ex.Message;
                return response;
            }
        }

        public async Task<bool> Registrar(RutasDto rutas)
        {
            var respuesta = false;
            if (rutas != null)
            {
                try
                {

                    var rutasDto = new RutasDto
                    {

                        Id_Ruta = rutas.Id_Unidad,
                        Id_Unidad = rutas.Id_Unidad,
                        Id_Usuario = rutas.Id_Usuario,
                        Lugar_Salida = rutas.Lugar_Salida,
                        Lugar_Destino = rutas.Lugar_Destino,
                        Fecha_Salidad = rutas.Fecha_Salidad,
                        Costo = rutas.Costo,
                        Lugares_Disponibles = rutas.Lugares_Disponibles,
                        Estatus = rutas.Estatus,

                    };
                    var rutaNueva = _mapper.Map<RutasDto, Rutas>(rutasDto);
                    await _context.Rutas.AddAsync(rutaNueva);
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

        public async Task<bool> ReservarLugar(ReservacionRequest reservacion)
        {
            var response = false;
            try
            {
                _context.Database.BeginTransaction();

                var ruta = _context.Rutas.Where(r => r.Id_Ruta.Equals(reservacion.Id_Ruta)).FirstOrDefault();
                var usuario = _context.Usuarios.Where(u => u.Id_Usuario.Equals(reservacion.Id_Usuario)).FirstOrDefault();

                if (ruta != null && usuario != null)
                {
                    if(ruta.Lugares_Disponibles >= reservacion.Num_Asientos)
                    {
                        ruta.Lugares_Disponibles -= reservacion.Num_Asientos;

                        _context.Reservaciones.Add(new Reservaciones
                        {
                            Id_Reservacion = Guid.NewGuid(),
                            Id_Ruta = reservacion.Id_Ruta,
                            Id_Usuario = reservacion.Id_Usuario,
                            Num_Asientos = reservacion.Num_Asientos,
                            Fecha_Reservacion = DateTime.Now,
                            Estatus = true
                        });

                        if(ruta.Lugares_Disponibles <= 0)
                        {
                            ruta.Estatus = false;
                        }

                        await _context.SaveChangesAsync();
                        _context.Database.CommitTransaction();
                        response = true;

                    }
                    else
                    {
                        throw new Exception("Número de Lugares insuficientes");
                    }
                }
                else
                {
                    throw new Exception("No existe una ruta o usuario con estos datos");
                }

                return response;
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                _context.Database.RollbackTransaction();
                throw new Exception("Ups, Ocurrio un error intenta mas tarde");
            }
        }

        public async Task<bool> EliminarManual(Guid id_Ruta)
        {
            try
            {
                var rutaAEliminar = await _context.Rutas.FindAsync(id_Ruta);

                if (rutaAEliminar == null)
                {
                    return false; // La unidad no existe
                }

                _context.Rutas.Remove(rutaAEliminar);
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

