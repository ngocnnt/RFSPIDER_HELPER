using System;
using System.Collections.Generic;
using System.Text;

namespace APP_KTRA_ROUTER.Models
{
   public  class TTinKDinh
    {
        public string MA_DVIQLY { get; set; }
        public string MA_DDO { get; set; }
        public string MA_KHANG { get; set; }
        public string MA_SOGCS { get; set; }
        public string MA_KVUC { get; set; }
        public string STT { get; set; }
        public DateTime NGAY_TTHAO { get; set; }
        public string MA_TBI { get; set; }
        public string SO_TBI { get; set; }
        public string MA_CLOAI { get; set; }
        public string SO_PHA { get; set; }
        public string DIEN_AP { get; set; }
        public string DONG_DIEN { get; set; }
        public string TYSO_DAU { get; set; }
        public string VH_CONG { get; set; }
        public string CAP_CXAC { get; set; }
        public Nullable<decimal> SO_DAY { get; set; }
        public string MA_BDONG { get; set; }
        public Nullable<decimal> SO_HUU { get; set; }
        public DateTime NGAY_KDINH { get; set; }
        public string SO { get; set; }
        public string MA_TCHI { get; set; }
        public DateTime HAN_KDINH { get; set; }
        public bool DA_THAO { get; set; }
        public bool NOT_DA_THAO { get; set; }

    }
}
