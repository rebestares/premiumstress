using System.Security.Cryptography;
using System.Text;

namespace Premiumstress.Blog.Services.Encryption
{
    public class EncryptionService : IEncryptionService
    {
        public string ConvertoToMd5(string text)
        {
            var md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(Encoding.ASCII.GetBytes(text));
            var result = md5.Hash;
            var str = new StringBuilder();
            foreach (var t in result)
            {
                str.Append(t.ToString("x2"));
            }

            return str.ToString();
        }
    }
}