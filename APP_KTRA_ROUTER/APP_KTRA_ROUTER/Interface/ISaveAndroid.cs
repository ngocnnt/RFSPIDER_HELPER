using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace APP_KTRA_ROUTER.Interface
{
    public interface ISaveAndroid
    {
       Task<string> SaveAndViewAsync(string fileName, String contentType, MemoryStream stream);
    }
}
