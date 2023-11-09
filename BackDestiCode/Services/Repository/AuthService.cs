using AutoMapper;
using BackDestiCode.Data.Context;
using BackDestiCode.Data.Models;
using BackDestiCode.DTOs;
using BackDestiCode.Security;
using BackDestiCode.Services.Interfaces;

namespace BackDestiCode.Services.Repository
{
    public class AuthService : IAuthService
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IEncrypt _encrypt;
        private AuthResponse _authResponse;

        public AuthService(ApiDbContext context, IMapper mapper, IJwtService jwtService, IEncrypt encrypt)
        {
            _context = context;
            _mapper = mapper;
            _jwtService = jwtService;
            _encrypt = encrypt;
            _authResponse = new AuthResponse();
        }

        public async Task<bool> Registrar(Usuarios usuario, DatosPersonales datosUsuario)
        {
            var respuesta = false;
            try
            {
                await _context.Usuarios.AddAsync(usuario);
                await _context.DatosPersonales.AddAsync(datosUsuario);

                await _context.SaveChangesAsync();
                respuesta = true;
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
                respuesta = false;
            }

            return respuesta;
        }
        
        public async Task<AuthResponse> Validar(AuthRequest request)
        {
            request.Contrasenia = _encrypt.AESEncrypt(request.Contrasenia);
            var usuario = _context.Usuarios.Where(x => 
                x.Correo.Equals(request.Correo) && 
                x.Contrasenia.Equals(request.Contrasenia))
                    .FirstOrDefault();
            
            if (usuario != null && usuario.Estatus == true)
            {
                var token = _jwtService.GenerateToken(usuario.Nombre_Usuario, usuario.Id_Usuario.ToString());

                _authResponse.Mensaje = "Ok";
                _authResponse.Resultado = true;
                _authResponse.Token = token;
            }
            else
            {
                _authResponse.Mensaje = "Credenciales Incorrectas";
                _authResponse.Resultado = false;
            }

            return _authResponse;
        }

        
    }
}
