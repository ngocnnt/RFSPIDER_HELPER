using APP_KTRA_ROUTER.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP_KTRA_ROUTER.Database
{
  public class db_TRAM : BindablelModel
    {
        public string MA_DVIQLY { get; set; }
        public string MA_TRAM { get; set; }
        public string TEN_TRAM { get; set; }
        public Int32 KY { get; set; }
        public Int32 THANG { get; set; }
        public Int32 NAM { get; set; }
        int status;
        public int STATUS
        {
            get => status;
            set
            {
                if (value != null)
                {
                    status = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
