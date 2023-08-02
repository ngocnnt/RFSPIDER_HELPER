using System;
using System.Collections.Generic;
using System.Text;

namespace APP_KTRA_ROUTER.Models
{
    class BienBanCrudApiModel
    {

        public string MaBienBan { get; set; }
        public string MaDonVi { get; set; }
        public string MaDVDL { get; set; }
        public string TrangThai { get; set; }
        public int SoLuong { get; set; }
        public string NguoiTao { get; set; }
        public string NgayTao { get; set; }
        public int TiepNhan { get; set; }
        public string NgayTiepNhan { get; set; }
        public string NguoiTiepNhan { get; set; }
        public int TraBaoHanh { get; set; }
        public string NgayTraBH { get; set; }
        public string NguoiTraBH { get; set; }
        public int NhanBaoHanh { get; set; }
        public string NgayNhanBH { get; set; }
        public string NguoiNhanBH { get; set; }
        public string GhiChu { get; set; }
        public List<ChiTietBienBanCrudApiModel> ListChiTietBienBan { get; set; }
    }
}
