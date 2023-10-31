using BackDestiCode.DTOs;
using BackDestiCode.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackDestiCode.Services.Repository
{
    public class JwtService : IJwtService
    {
        private readonly AppJwtOptions _jwtOptions;

        public JwtService(IOptions<AppJwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateToken(string NameUser, string IdUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            var Claimlist = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, IdUser),
                new Claim(JwtRegisteredClaimNames.Name, NameUser)
            };

            var TokenDescriptor = new SecurityTokenDescriptor
            {
                /*Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,*/
                Subject = new ClaimsIdentity(Claimlist),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var Token = tokenHandler.CreateToken(TokenDescriptor);

            return tokenHandler.WriteToken(Token);
        }
    }
}
