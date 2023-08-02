using System;
using System.Collections.Generic;
using System.Text;

namespace APP_KTRA_ROUTER.Models
{
    class ChiTietBienBanCrudApiModel
    {
        public int MaBBBH { get; set; }
        public int MaChiTietBBBH { get; set; }
        public decimal? SerialId { get; set; }
        public string MaChungLoai { get; set; }
        public string NgayXuatKho { get; set; }
        public string HanBaoHanh { get; set; }
        public bool LoaiBaoHanh { get; set; }
        public string MoTa { get; set; }
        public int TinhTrang { get; set; }
        public int TraBaoHanh { get; set; }
        public string NgayTraBaoHanh { get; set; }
        public int NhanBaoHanh { get; set; }
        public string NgayNhanBaoHanh { get; set; }
    }
}
