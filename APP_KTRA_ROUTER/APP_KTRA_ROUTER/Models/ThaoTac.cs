using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APP_KTRA_ROUTER.Models
{
    public class ThaoTac
    {
        [Key]
        public string LOAI_THAO_TAC { get; set; }
        public string TEN_THAO_TAC { get; set; }
        public ThaoTac(string loaiTT, string tenTT)
        {
            LOAI_THAO_TAC = loaiTT;
            TEN_THAO_TAC = tenTT;
        }
    }
}
