using AutoMapper;
using BackDestiCode.Data.Context;
using BackDestiCode.Data.Models;
using BackDestiCode.DTOs;
using BackDestiCode.Services.Interfaces;

namespace BackDestiCode.Services.Repository
{
    public class ServiceUsuario : IServiceUsuario
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        public ServiceUsuario(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Registrar(UsuariosDto usuario)
        {
            var respuesta = false;
            if (usuario != null) 
            {
                try
                {
                    var id = Guid.NewGuid();
                    var usuarioDto = new UsuariosDto
                    {
                        Id_Usuario = id,
                        Nombre_Usuario = usuario.Nombre_Usuario,
                        Correo = usuario.Correo,
                        Fecha_Registro = usuario.Fecha_Registro,
                        Contrasenia = usuario.Contrasenia,
                        Token = usuario.Token
                    };
                    var usuarioNuevo = _mapper.Map<UsuariosDto, Usuarios>(usuarioDto);
                    await _context.Usuarios.AddAsync(usuarioNuevo);
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
        public async Task<bool> Validar(string nombreUsuario, string contrasenia)
        {
            var respuesta = false;
            var usuario = _context.Usuarios.Where(x => x.Nombre_Usuario == nombreUsuario && x.Contrasenia == contrasenia).FirstOrDefault();
            if (usuario != null)
            {
                respuesta = true;
            } else { respuesta = false; }
            return respuesta;
        }
    }
}
