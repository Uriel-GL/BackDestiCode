using AutoMapper;
using BackDestiCode.Data.Context;
using BackDestiCode.Data.Models;
using BackDestiCode.DTOs;
using BackDestiCode.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace BackDestiCode.Services.Repository
{
    public class ServiceDatosPersona : IServiceDatosPersona
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        public ServiceDatosPersona(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<DatosPersonalesDto>> GetUsuarios()
        {
            List<DatosPersonalesDto> usuariosDto = new List<DatosPersonalesDto>();
            try
            {
                var usuarios = await _context.DatosPersona.ToListAsync();
                usuariosDto = _mapper.Map<List<DatosPersonales>, List<DatosPersonalesDto>>(usuarios);

            }
            catch (Exception ex)
            {

                string msg = ex.Message;
            }
            return usuariosDto;
        }

        public async Task<bool> InsertUsuario(DatosPersonalesDto usuario)
        {
            var respuesta = false;
            if (usuario != null)
            {
                try
                {
                    var estatus = true;
                    var id = Guid.NewGuid();
                    var usuarioDto = new DatosPersonalesDto { 
                        Nombre_Completo = usuario.Nombre_Completo,
                        
                        Credencial = usuario.Credencial,
                        Fecha_Nacimiento = usuario.Fecha_Nacimiento,
                        Correo = usuario.Correo,
                        Grupo = usuario.Grupo,
                        Matricula = usuario.Matricula,
                        Telefono = usuario.Telefono,
                        Universidad = usuario.Universidad,
                        Estatus = estatus,
                        Id_Usuario = id
                    };
                    var usuarioNuevo = _mapper.Map<DatosPersonalesDto, DatosPersonales>(usuarioDto);
                    await _context.DatosPersona.AddAsync(usuarioNuevo);
                    await _context.SaveChangesAsync();
                    respuesta = true;
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    respuesta = false;
                }
            } else { respuesta = false; }
            return respuesta;
        }

        public async Task<bool> UpdateUsuario(DatosPersonalesDto usuario)
        {
            var respuesta = false;
            var user = await _context.DatosPersona.FindAsync(usuario.Id_Usuario);
            if (user != null)
            {
                try
                {
                    var usuarioDto = new DatosPersonalesDto
                    {
                        Nombre_Completo = usuario.Nombre_Completo,
                        Correo = usuario.Correo,
                        Credencial = usuario.Credencial,
                        Fecha_Nacimiento = usuario.Fecha_Nacimiento,
                        Grupo = usuario.Grupo,
                        Matricula = usuario.Matricula,
                        Telefono = usuario.Telefono,
                        Universidad = usuario.Universidad,
                        Estatus = usuario.Estatus,
                        Id_Usuario = usuario.Id_Usuario
                    };
                    _mapper.Map<DatosPersonalesDto, DatosPersonales>(usuarioDto);
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

        public async Task<bool> DeleteUsuario(Guid Id_Usuario)
        {
            var respuesta = false;
            var usuario = await _context.DatosPersona.FindAsync(Id_Usuario);
            if (usuario != null)
            {
                try
                {
                    var usuarioDto = new DatosPersonalesDto
                    {
                        Nombre_Completo = usuario.Nombre_Completo,
                        Correo = usuario.Correo,
                        Credencial = usuario.Credencial,
                        Fecha_Nacimiento = usuario.Fecha_Nacimiento,
                        Grupo = usuario.Grupo,
                        Matricula = usuario.Matricula,
                        Telefono = usuario.Telefono,
                        Universidad = usuario.Universidad,
                        Estatus = false,
                        Id_Usuario = usuario.Id_Usuarios
                    };
                    _mapper.Map<DatosPersonalesDto, DatosPersonales>(usuarioDto);
                    await _context.SaveChangesAsync();
                    respuesta = true;
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    respuesta = false;
                }
            }
            return respuesta;
        }
    }
}
