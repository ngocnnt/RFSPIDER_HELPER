using System;
using System.Collections.Generic;
using System.Text;

namespace APP_KTRA_ROUTER.Models
{
    public class INFO_MODEM
    {
        public string ATTRDATAID { get; set; }
        public string OBJTYPEID { get; set; }
        public string OBJID { get; set; }
        public string OUTSTATION { get; set; }
        public string METER_TYPE { get; set; }
        public string METHOD { get; set; }
        public string PORT { get; set; }
        public string TCP_IP { get; set; }
        public string TCP_PORT { get; set; }
        public int TU { get; set; }
        public int TI { get; set; }
        public string UNIT_DISPLAY { get; set; }
        public int KIEU_DOC { get; set; }
        public string PORT_SETTING { get; set; }
        public string KIEU_DAUDAY { get; set; }
        public string LOAI_DDO { get; set; }
        public double IMPORTKWH { get; set; }
        public double EXPORTKWH { get; set; }
        public double OVER_CURRENT { get; set; }
        public double OVER_AP { get; set; }
        public double DONG_DIEN { get; set; }
        public string TYSO_DAU_TI { get; set; }
        public string TYSO_DAU_TU { get; set; }
        public string ID_DONVIGIAO { get; set; }
        public string ID_DONVINHAN { get; set; }
        public string IMEI { get; set; }
        public string STATUS_MODEM { get; set; }
        public int DIEN_AP { get; set; }
        public string KIEU_CTO { get; set; }
        public int MA_GC { get; set; }
        public string MA_COT { get; set; }
        public string MA_CTO { get; set; }
        public DateTime STATUS_MODEM_TIME { get; set; }
        public string MODEM_NHACC { get; set; }
        public string MA_DAI_CMIS { get; set; }
        public string SO_DIENTHOAI { get; set; }
        public int MESH { get; set; }
        public int CONG_SUAT { get; set; }
        public string CTO_TONG { get; set; }
    }
    public class HISTORY
    {
        public string MaDDo { get; set; }
        public string IMEICu { get; set; }
        public string IMEIMoi { get; set; }
        public string NguoiSua { get; set; }
    }
    public class HISTORY_INFO
    {
        public string MA_DDO { get; set; }
        public string IMEI_CU { get; set; }
        public string IMEI_MOI { get; set; }
        public DateTime NGAY_SUA { get; set; }
    }
}
