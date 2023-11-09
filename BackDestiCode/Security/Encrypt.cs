using System.Security.Cryptography;
using System.Text;

namespace BackDestiCode.Security
{
    public class Encrypt : IEncrypt
    {
        private static readonly byte[] KEY = new byte[] { 44, 73, 31, 74, 33, 63, 48, 50, 34, 72, 40, 33, 31, 48, 30, 67 };
        private static readonly byte[] IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private readonly Aes managed = Aes.Create();

        public Encrypt()
        {
            managed.Mode = CipherMode.CBC;
            managed.Key = KEY;
            managed.IV = IV;
        }

        public string AESEncrypt(string texto)
        {
            ICryptoTransform encrypto = managed.CreateEncryptor(managed.Key, managed.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(texto);
                        }
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            
        }
        public string AesDecrypt(string textoEncriptado)
        {
            ICryptoTransform decrypt = managed.CreateDecryptor(managed.Key, managed.IV);
            byte[] datosEncriptados = Convert.FromBase64String(textoEncriptado);

            using (MemoryStream ms = new MemoryStream(datosEncriptados))
            {
                using (CryptoStream cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read))
                {
                    using (MemoryStream msResultado = new MemoryStream())
                    {
                        byte[] buffer = new byte[1024]; // Elige un tamaño adecuado para el búfer
                        int bytesRead;

                        while ((bytesRead = cs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            msResultado.Write(buffer, 0, bytesRead);
                        }

                        return Encoding.UTF8.GetString(msResultado.ToArray());
                    }
                }
            }
        }

    }
}
