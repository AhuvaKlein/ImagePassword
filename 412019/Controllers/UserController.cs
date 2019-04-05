using _412019.Models;
using ImagesPassword.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace _412019.Controllers
{
    public class UserController : Controller
    {
        ImageManager mgr = new ImageManager(Properties.Settings.Default.ConStr);

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User user, string password)
        {
            mgr.SignUp(user, password);

            return Redirect("/user/login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user, string password)
        {
            User u = mgr.Login(user, password);
            if (u == null)
            {
                return Redirect("/user/login");
            }

            FormsAuthentication.SetAuthCookie(user.Email, true);
            return Redirect("/home/index");
        }

        public ActionResult MyAccount()
        {
            MyAccountViewModel vm = new MyAccountViewModel();
            
            if (User.Identity.IsAuthenticated)
            {
                User user = mgr.GetUserByEmail(User.Identity.Name);
                vm.User = user;
                vm.Images = mgr.GetImageForUser(user.Id);
            }

            return View(vm);
        }
    }
}