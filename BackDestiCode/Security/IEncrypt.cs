namespace BackDestiCode.Security
{
    public interface IEncrypt
    {
        string AESEncrypt(string texto);
        string AesDecrypt(string textoEncriptado);
    }
}
