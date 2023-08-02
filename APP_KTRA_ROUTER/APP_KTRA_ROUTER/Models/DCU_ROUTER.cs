using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace APP_KTRA_ROUTER.Models
{
   public class DCU_ROUTER
    {
        public string Type { get; set; }
        public string DcuID { get; set; }
        public string TEN_TRAM { get; set; }
        public string DIA_CHI { get; set; }         
        public string MA_COT { get; set; }
        public DateTime? NGAY_KBAO { get; set; }
        public string Path { get; set; } 
        public string MeterID { get; set; }
        public string STATUS { get; set; }
        public decimal? KWH { get; set; } 
    }

    public class DcuMqttReq
    {
        public string TenDangNhap { get; set; }
        public string MaDviQly { get; set; }
        public string MaTram { get; set; }
        public string Type { get; set; }
        public UInt32 DcuID { get; set; }
        public string MeterID { get; set; }
        public string Path { get; set; }
        public string TypeReq { get; set; } 
    }
    public class DcuMqttResp
    {
        public string TenDangNhap { get; set; }
        public string MaDviQly { get; set; }
        public string MaTram { get; set; }
        public string Type { get; set; }
        public UInt32 DcuID { get; set; }
        public string MeterID { get; set; }
        public string TrangThai { get; set; }
        public string CSQ { get; set; }
        public string Chiso { get; set; }
        public int RSSI { get; set; }
        public string NgayGio { get; set; }
        public string ErrorCode { get; set; }
        string _typereg;
        public string TypeReq { get =>  _typereg; set { if (value == null) _typereg = ""; else _typereg = value;  } }
        public string PATH { get; set; }
        public GridLength RowHeight { get; set; }
    }

    public class PATH
    {
        public string SPIDER_PATH { get; set; }
    }

}
