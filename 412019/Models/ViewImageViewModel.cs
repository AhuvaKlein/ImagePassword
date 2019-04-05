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

    public class UploadViewModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsAuthenticated { get; set; }
    }

    public class MyAccountViewModel
    {
        public IEnumerable<Image> Images { get; set; }
        public User User { get; set; }
    }
}