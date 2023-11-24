using AutoMapper;
using BackDestiCode.Data.Context;
using BackDestiCode.Data.Models;
using BackDestiCode.DTOs;
using BackDestiCode.Security;
using BackDestiCode.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace BackDestiCode.Services.Repository
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IServiceUnidad _serviceUnidad;
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEncrypt _encrypt;
        public UsuarioService(ApiDbContext context, IMapper mapper, IServiceUnidad serviceUnidad, IEncrypt encrypt)
        {
            _context = context;
            _mapper = mapper;
            _serviceUnidad = serviceUnidad;
            _encrypt = encrypt;
        }

        public async Task<bool> UpdateUsuario(AuthRegister datos)
        {
            var respuesta = false;
            try
            {
                var usuario = await _context.Usuarios.Where(u => u.Id_Usuario.Equals(datos.Usuario.Id_Usuario))
                .FirstOrDefaultAsync();
                var datosUsuario = await _context.DatosPersonales.Where(dp => dp.Id_Usuario.Equals(datos.DatosPersonales.Id_Usuario)).FirstOrDefaultAsync();

                if (usuario != null)
                {
                    usuario.Nombre_Usuario = datos.DatosPersonales.Nombre_Completo;
                    usuario.Correo = datos.Usuario.Correo;
                    usuario.Contrasenia = _encrypt.AESEncrypt(datos.Usuario.Contrasenia);

                    datosUsuario.Nombre_Completo = datos.DatosPersonales.Nombre_Completo;
                    datosUsuario.Fecha_Nacimiento = datos.DatosPersonales.Fecha_Nacimiento;
                    datosUsuario.Matricula = datos.DatosPersonales.Matricula;
                    datosUsuario.Grupo = datos.DatosPersonales.Grupo;
                    datosUsuario.Credencial = datos.DatosPersonales.Credencial;
                    datosUsuario.Universidad = datos.DatosPersonales.Universidad;
                    datosUsuario.Telefono = datos.DatosPersonales.Telefono;
                    datosUsuario.Correo = datos.DatosPersonales.Correo;

                    await _context.SaveChangesAsync();
                    respuesta = true;
                }
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
                respuesta = false;
            }

            return respuesta;
        }

        public async Task<bool> DeleteUsuario(Guid Id_Usuario)
        {
            var respuesta = false;
            var usuario = await _context.Usuarios.FindAsync(Id_Usuario);
            if (usuario != null)
            {
                try
                {
                    usuario.Estatus = false;
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

        public async Task<DatosPersonalesDto> GetUsuarioById(Guid Id_Usuario)
        {
            try
            {
                var response = await _context.DatosPersonales.Where(x => x.Id_Usuario.Equals(Id_Usuario))
                    .Select(datos => new DatosPersonalesDto
                    {
                        Id_DatosPersonales = datos.Id_DatosPersonales,
                        Estatus = datos.Estatus,
                        Nombre_Completo = datos.Nombre_Completo,
                        Id_Usuario = datos.Id_Usuario,
                        Universidad = datos.Universidad,
                        Telefono = datos.Telefono,
                        Fecha_Nacimiento = datos.Fecha_Nacimiento,
                        Grupo = datos.Grupo,
                        Matricula = datos.Matricula,
                        Correo = datos.Correo,
                        Credencial = datos.Credencial,
                        Usuarios = new Usuarios
                        {
                            Correo = datos.Correo,
                            Nombre_Usuario = datos.Usuarios.Nombre_Usuario,
                            Contrasenia = _encrypt.AesDecrypt(datos.Usuarios.Contrasenia),
                            Id_Usuario = datos.Id_Usuario,
                            Estatus = datos.Usuarios.Estatus
                        }
                    }).FirstOrDefaultAsync();

                return response;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw new Exception("Ocurrio un error al consultar tus datos intenta mas tarde");
            }
        }
    }
}
