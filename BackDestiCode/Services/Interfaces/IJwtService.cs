namespace BackDestiCode.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string NameUser, string IdUser);
    }
}
