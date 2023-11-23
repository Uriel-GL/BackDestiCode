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

                        Id_Ruta = Guid.NewGuid(),
                        Id_Unidad = rutas.Id_Unidad,
                        Id_Usuario = rutas.Id_Usuario,
                        Lugar_Salida = rutas.Lugar_Salida,
                        Lugar_Destino = rutas.Lugar_Destino,
                        Fecha_Salida = rutas.Fecha_Salida,
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
                var reservaciones = await _context.Reservaciones.Where(x => x.Id_Ruta.Equals(id_Ruta)).ToListAsync();

                // Eliminar las reservaciones asociadas a la ruta
                foreach (var reservacion in reservaciones)
                {
                    _context.Reservaciones.Remove(reservacion);
                }

                var rutaAEliminar = await _context.Rutas.FindAsync(id_Ruta);

                if (rutaAEliminar == null)
                {
                    return false; // La ruta no existe
                }

                _context.Rutas.Remove(rutaAEliminar);
                await _context.SaveChangesAsync();
                return true; // Éxito al eliminar la ruta
            }
            catch (Exception ex)
            {
                // Registra la excepción para depuración
                var msg = ex.Message;
                return false; // Error al intentar eliminar la ruta
            }
        }

        public async Task<List<RutasDto>> GetAllRutas()
        {
            try
            {
                var response = await _context.Rutas.ToListAsync();
                var data = _mapper.Map<List<RutasDto>>(response);

                return data;

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw new Exception("Ocurrio un error al consultar las rutas, intenta mas tarde " + msg);
            }
        }

        public async Task<List<RutasDto>> GetRutasByIdUsuario(Guid Id_Usuario)
        {
            try
            {
                var response = await _context.Rutas.Where(fk => fk.Id_Usuario.Equals(Id_Usuario)).ToListAsync();

                var data = _mapper.Map<List<Rutas>, List<RutasDto>>(response).ToList();

                return data;
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                throw new Exception("Ocurrio un error al consultar tus rutas, intenta mas tarde");
            }
        }

        public async Task<RutasDto> GetRutaByIdRuta(Guid Id_Ruta)
        {
            try
            {
                var response = await _context.Rutas.Where(pk => pk.Id_Ruta.Equals(Id_Ruta)).FirstOrDefaultAsync();
                var data = _mapper.Map<Rutas, RutasDto>(response);

                var usuario = _context.Usuarios.Include(p => p.DatosPersonales).Where(pk => pk.Id_Usuario.Equals(data.Id_Usuario)).FirstOrDefault();
                var vehiculo = _context.Vehiculos.Include(p => p.Usuarios).Where(pk => pk.Id_Unidad.Equals(data.Id_Unidad)).FirstOrDefault();

                data.Usuarios = new Usuarios 
                {
                    Id_Usuario = usuario.Id_Usuario,
                    Nombre_Usuario = usuario.Nombre_Usuario,
                    Correo = usuario.Correo,
                    Estatus = usuario.Estatus,
                };
                data.Vehiculos = new Vehiculos 
                {
                    Id_Unidad = vehiculo.Id_Unidad,
                    Id_Usuario = vehiculo.Id_Usuario,
                    Color = vehiculo.Color,
                    Imagen = vehiculo.Imagen,
                    Modelo = vehiculo.Modelo,
                    Placa = vehiculo.Placa
                };

                return data;
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                throw new Exception("Ocurrio un error al consultar la ruta intenta mas tarde");
            }
        }

        public async Task<List<UsuariosDto>> GetUsersByReservacion(Guid Id_Ruta)
        {
            try
            { 
                var usuarios = await _context.Reservaciones.Where(x => x.Id_Ruta.Equals(Id_Ruta))
                    .Include(x => x.Usuarios)
                        .ThenInclude(d => d.DatosPersonales)
                    .Select(reservacion => new UsuariosDto
                    {
                        Nombre_Usuario = reservacion.Usuarios.Nombre_Usuario,
                        Correo = reservacion.Usuarios.Correo,
                        DatosPersonales = reservacion.Usuarios.DatosPersonales.Select(dp => new DatosPersonales
                        {
                            Telefono = dp.Telefono,
                            Grupo = dp.Grupo,
                            Nombre_Completo = dp.Nombre_Completo,
                        }).ToList()

                    }).ToListAsync();

                return usuarios;
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                throw new Exception("Ocurrio un error al consultar los usuarios que reservaron");
            }
        }

        public async Task<List<ReservacionesDto>> GetTicketsReservacion(Guid Id_Usuario)
        {
            try
            {
                //var respuesta = await _context.Reservaciones.Where(x => x.Id_Usuario.Equals(Id_Usuario))
                //    .Select(reservacion => new RutasDto
                //    {
                //        Id_Ruta = reservacion.Id_Ruta,
                //        Id_Usuario = reservacion.Rutas.Id_Usuario,
                //        Fecha_Salida = reservacion.Rutas.Fecha_Salida,
                //        Lugar_Salida = reservacion.Rutas.Lugar_Salida,
                //        Lugar_Destino = reservacion.Rutas.Lugar_Destino,
                //    }).ToListAsync();

                var response = await _context.Reservaciones.Where(x => x.Id_Usuario.Equals(Id_Usuario))
                    .Select(r => new ReservacionesDto
                    {
                        Id_Reservacion = r.Id_Reservacion,
                        Num_Asientos = r.Num_Asientos,
                        Id_Ruta = r.Id_Ruta,
                        Id_Usuario = r.Id_Usuario,
                        Rutas = new Rutas
                        {
                            Id_Ruta = r.Rutas.Id_Ruta,
                            Id_Usuario = r.Rutas.Id_Usuario,
                            Fecha_Salida = r.Rutas.Fecha_Salida,
                            Lugar_Salida = r.Rutas.Lugar_Salida,
                            Lugar_Destino = r.Rutas.Lugar_Destino
                        }
                    }).ToListAsync();

                return response;
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                throw new Exception("Ocurrio un error al intenar consultar tus reservaciones");
            }
        }

    }
}

