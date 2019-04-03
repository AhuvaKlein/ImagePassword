using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImagesPassword.Data;

namespace _412019.Models
{
    public class ViewImageViewModel
    {
        public Image Image { get; set; }
        public bool IncorrectPassword { get; set; }
        public string Password { get; set; }
    }
}