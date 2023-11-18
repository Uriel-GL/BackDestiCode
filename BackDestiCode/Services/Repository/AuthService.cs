using AutoMapper;
using BackDestiCode.Data.Context;
using BackDestiCode.Data.Models;
using BackDestiCode.DTOs;
using BackDestiCode.Security;
using BackDestiCode.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace BackDestiCode.Services.Repository
{
    public class AuthService : IAuthService
    {
        private readonly ApiDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IEncrypt _encrypt;
        private AuthResponse _authResponse;
        private readonly IOptions<ConfiguracionesCorreo> _configuracionesCorreoOptions;
        private readonly ConfiguracionesCorreo _configuracionesCorreo;
        private readonly ITokenDiccionario _tokenDiccionario;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ApiDbContext context, IJwtService jwtService, IEncrypt encrypt, 
            IOptions<ConfiguracionesCorreo> configuracionesCorreoOptions, ITokenDiccionario 
            tokenDiccionario, ILogger<AuthService> logger
            )
        {
            _context = context;
            _jwtService = jwtService;
            _encrypt = encrypt;
            _authResponse = new AuthResponse();
            _configuracionesCorreoOptions = configuracionesCorreoOptions;
            _configuracionesCorreo = configuracionesCorreoOptions.Value;
            _tokenDiccionario = tokenDiccionario;
            _logger = logger;
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
                _authResponse.Usuario = usuario.Id_Usuario;
            }
            else
            {
                _authResponse.Mensaje = "Credenciales Incorrectas";
                _authResponse.Resultado = false;
            }

            return _authResponse;
        }

        public async Task<bool> EnviarCorreo(CorreoRequest request)
        {
            var respuesta = false;
            try
            {
                var credenciales = new NetworkCredential(_configuracionesCorreo.Mail, _configuracionesCorreo.Password);
                var correo = new MailMessage()
                {
                    From = new MailAddress(_configuracionesCorreo.Mail, _configuracionesCorreo.DisplayName),
                    Subject = request.Subject,
                    Body = request.Body,
                    IsBodyHtml = true
                };
                correo.To.Add(new MailAddress(request.ToMail));
                var cliente = new SmtpClient()
                {
                    Port = _configuracionesCorreo.Port,
                    Host = _configuracionesCorreo.Host,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = credenciales
                };
                cliente.Send(correo);
                _logger.LogInformation($"Correo enviado a {request.ToMail} con éxito.");
                respuesta = true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error al enviar el correo a {request.ToMail}: {ex.Message}");
                respuesta = false;
            }
            return respuesta;
        }

        public async Task<bool> RecuperarContrasenia(RecuperarContraseniaRequest request)
        {
            var respuesta = false;
            try
            {
                var recoveryToken = Guid.NewGuid().ToString();

                var usuario = _context.Usuarios.FirstOrDefault(x => x.Correo == request.Correo);
                if (usuario != null)
                {
                    _tokenDiccionario.Tokens[request.Correo] = recoveryToken;

                    var emailBody = $"Hola, has solicitado restablecer tu contraseña. Tu código de recuperación es: {recoveryToken}";

                    CorreoRequest correo = new CorreoRequest
                    {
                        Body = emailBody,
                        Subject = "Recuperación de contraseña.",
                        ToMail = request.Correo
                    };
                    if (await EnviarCorreo(correo))
                    {
                        respuesta = true;
                    }
                    else
                    {
                        respuesta = false;
                    }
                }
                else
                {
                    respuesta = false;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                respuesta = false;
            }
            return respuesta;
        }

        public async Task<bool> VerificarTokenRecuperacion(RecuperarContraseniaRequest request)
        {
            if(_tokenDiccionario.Tokens.TryGetValue(request.Correo, out var storedToken) && storedToken == request.Token)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ResetearContrasenia(RecuperarContraseniaRequest request)
        {
            if (await VerificarTokenRecuperacion(request))
            {
                var usuario = _context.Usuarios.FirstOrDefault(x =>  x.Correo == request.Correo);
                if (usuario != null)
                {
                    usuario.Contrasenia = _encrypt.AESEncrypt(request.Contrasenia);
                    await _context.SaveChangesAsync();

                    _tokenDiccionario.Tokens.Remove(request.Correo);
                    return true;
                }
            }
            return false;
        }
        
    }
}
