﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ServerBTS2.Models
{
    public class HinhAnhTram
    {
        [Key]
        public int IDHinhAnh { get; set; }
        [Required(ErrorMessage = "IDTram is required")]
        public int IDTram { get; set; }
        [Required(ErrorMessage = "Ten is required")]
        [RegularExpression(@"^.*\.(jpg|jpeg|gif|JPG|png|PNG)$", ErrorMessage = "Image is not valid")]
        public string Ten { get; set; }
    }
}