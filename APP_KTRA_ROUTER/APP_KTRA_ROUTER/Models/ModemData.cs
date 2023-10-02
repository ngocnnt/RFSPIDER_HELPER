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
        public bool ChuyenMD { get; set; }
        public int LoaiThay { get; set; }
    }
    public class HISTORY_INFO
    {
        public string MA_DDO { get; set; }
        public string IMEI_CU { get; set; }
        public string IMEI_MOI { get; set; }
        public DateTime NGAY_SUA { get; set; }
        public bool CHUYEN_MAYDOC { get; set; }

    }
    public class INFO_CONGTO
    {
        public string ASSETID { get; set; }
        public string ASSETID_PARENT { get; set; }
        public string ASSETID_ORG { get; set; }
        public string ASSETDESC { get; set; }
        public int ASSETORD { get; set; }
        public string SITEID { get; set; }
        public string CATEGORYID { get; set; }
        public string TYPEID { get; set; }
        public string MANDEPTIDLIST { get; set; }
        public string USESTATUS_LAST_ID { get; set; }
        public DateTime USESTATUS_LAST_DTIME { get; set; }
        public string SERIALNUM { get; set; }
        public string P_MANUFACTURERID { get; set; }
        public string P_VENDORID { get; set; }
        public DateTime P_INSTALLDATE { get; set; }
        public double P_PRICE { get; set; }
        public string P_PRICEUOM { get; set; }
        public double P_PRICE_INC_ALL_CHILD { get; set; }
        public int PRIORITY { get; set; }
        public string USER_CR_ID { get; set; }
        public DateTime USER_CR_DTIME { get; set; }
        public string USER_MDF_ID { get; set; }
        public DateTime USER_MDF_DTIME { get; set; }
        public string USESTATUS_UPDATE_ID { get; set; }
        public string ASSETIDSORT { get; set; }
        public string ORGID { get; set; }
        public string ULEVELID { get; set; }
        public string ASSETNOTE { get; set; }
        public DateTime DATEOPRATOIN { get; set; }
        public string ASSETID_LINK { get; set; }
        public string ASSETID_LINK1 { get; set; }
        public string ASSETID_LINK2 { get; set; }
        public string ASSETID_LINK3 { get; set; }
        public int DATEMANUFACTURE { get; set; }
        public string NATIONALFACT { get; set; }
        public string OWNER { get; set; }
        public string OWNER_LAST { get; set; }
        public DateTime OWNER_LAST_DTIME { get; set; }
        public string URL { get; set; }
        public int ASSETLEVEL { get; set; }
        public string ASSETINFO { get; set; }
        public string ASSETID_FMIS { get; set; }
        public string ASSETID_CMIS { get; set; }
    }
    public class USER_INFO
    {
        public string USER_NAME { get; set; }
        public string DON_VI { get; set; }
        public DateTime NGAY_TẠO { get; set; }
    }

    public class HISTORY_TRANGTHAI
    {
        public string Imei { get; set; }
        public string NguoiKT { get; set; }
        public string TrangThaiKB { get; set; }
        public string TrangThaiMD { get; set; }
        public DateTime LastTimeCN { get; set; }
        public string CSQ { get; set; }
        public string IPModem { get; set; }
        public string IPServer { get; set; }
        public string CountCN { get; set; }
    }
    public class HISTORY_DOCCTO
    {
        public string Imei { get; set; }
        public string NguoiKT { get; set; }
        public string LoaiDoc { get; set; }
        public string KetQua { get; set; }
    }
}
