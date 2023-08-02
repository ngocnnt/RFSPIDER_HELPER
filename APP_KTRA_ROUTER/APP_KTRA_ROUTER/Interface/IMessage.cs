using System;
using System.Collections.Generic;
using System.Text;

namespace APP_KTRA_ROUTER.Interface
{
  public   interface  IMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
