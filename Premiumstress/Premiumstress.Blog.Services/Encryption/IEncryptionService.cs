using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Premiumstress.Blog.Services.Encryption
{
    public interface IEncryptionService
    {
        string ConvertoToMd5(string text);
    }
}
