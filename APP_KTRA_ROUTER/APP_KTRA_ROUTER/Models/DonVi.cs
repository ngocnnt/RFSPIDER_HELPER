using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APP_KTRA_ROUTER.Models
{
    public class DonVi
    {
        [Key]
        public string MA_DON_VI { get; set; }
        public string TEN_DON_VI { get; set; } 
    }
}
