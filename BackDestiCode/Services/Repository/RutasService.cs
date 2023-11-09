using AutoMapper;
using BackDestiCode.Data.Context;
using BackDestiCode.Data.Models;
using BackDestiCode.DTOs;
using BackDestiCode.Services.Interfaces;

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

    }


}

