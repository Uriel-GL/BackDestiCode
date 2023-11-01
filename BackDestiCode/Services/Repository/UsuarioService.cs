using AutoMapper;
using BackDestiCode.Data.Context;
using BackDestiCode.Data.Models;
using BackDestiCode.DTOs;
using BackDestiCode.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace BackDestiCode.Services.Repository
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        public UsuarioService(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> UpdateUsuario(AuthRegister datos)
        {
            var respuesta = false;
            try
            {
                var usuario = await _context.Usuarios.Where(u => u.Id_Usuario.Equals(datos.Usuario.Id_Usuario))
                .FirstOrDefaultAsync();
                var datosUsuario = await _context.DatosPersonales.FindAsync(datos.Usuario.Id_Usuario);

                if (usuario != null)
                {
                    usuario.Nombre_Usuario = datos.Usuario.Nombre_Usuario;
                    usuario.Correo = datos.Usuario.Correo;
                    usuario.Contrasenia = datos.Usuario.Contrasenia;

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
    }
}
