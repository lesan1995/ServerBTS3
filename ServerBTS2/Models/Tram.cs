using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ServerBTS2.Models
{
    public class Tram
    {
        [Key]
        public int IDTram { get; set; }
        [Required(ErrorMessage = "TenTram is required")]
        public string TenTram { get; set; }
        [Range(0,15,ErrorMessage = "CotAnten should be in 0 to 15 range.")]
        public int CotAnten { get; set; }
        [Range(0, 15, ErrorMessage = "CotTiepDat should be in 0 to 15 range.")]
        public int CotTiepDat { get; set; }
        [Required(ErrorMessage = "Tinh is required")]
        [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "Tinh should be in 000000 to 999999 range")]
        public string Tinh { get; set; }
        [Range(-85, 85, ErrorMessage = "ViDo should be in -85 to 85 range.")]
        public double ViDo { get; set; }
        [Range(-180, 180, ErrorMessage = "KinhDo should be in -180 to 180 range.")]
        public double KinhDo { get; set; }
        
        [Required(ErrorMessage = "IDQuanLy is required")]
        public string IDQuanLy { get; set; }
        [Range(0, 100, ErrorMessage = "BanKinhPhuSong should be bigger than 0.")]
        public double BanKinhPhuSong { get; set; }
    }
}