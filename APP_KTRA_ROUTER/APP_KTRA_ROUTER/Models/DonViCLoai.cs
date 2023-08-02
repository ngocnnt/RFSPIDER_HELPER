using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APP_KTRA_ROUTER.Models
{
    public class DonViCLoai
    {
        [Key]
        public string MA_CLOAI { get; set; }
        public string TEN_CLOAI { get; set; } 
    }
}
