using BackDestiCode.Services.Interfaces;

namespace BackDestiCode.Services.Repository
{
    public class TokenDiccionario : ITokenDiccionario
    {
        public Dictionary<string, string> Tokens { get; set; } = new Dictionary<string, string>();
    }
}
