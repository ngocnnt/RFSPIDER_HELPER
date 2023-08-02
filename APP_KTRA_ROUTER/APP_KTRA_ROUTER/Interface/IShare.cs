using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace APP_KTRA_ROUTER.Interface
{
    public interface IShare
    {
        Task Show(string title, string message, string filePath);
    }
}
