using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APP_KTRA_ROUTER.Models
{
    public class LyDoThao
    {
        [Key]
        public string LY_DO_THAO { get; set; }
        public LyDoThao(string lyDoThao)
        {
            LY_DO_THAO = lyDoThao;
        }
    }
}
