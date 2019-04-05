using _412019.Models;
using ImagesPassword.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace _412019.Controllers
{
    public class HomeController : Controller
    {
        ImageManager mgr = new ImageManager(Properties.Settings.Default.ConStr);

        [Authorize]
        public ActionResult Index()
        {
            UploadViewModel vm = new UploadViewModel();
            vm.IsAuthenticated = User.Identity.IsAuthenticated;
            if (vm.IsAuthenticated)
            {
                User u = mgr.GetUserByEmail(User.Identity.Name);
                vm.UserId = u.Id;
                vm.Name = u.Name;
                vm.Email = u.Email;
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult SubmitImage(HttpPostedFileBase image, string password, int userId)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            string fullName = $"{Server.MapPath("/UploadedImages")}\\{fileName}";
            image.SaveAs(fullName);
            Image img = new Image
            {
                FileName = fileName,
                Password = password,
                UserId = userId
            };
            int id = mgr.AddImage(img);

            img.Id = id;


            return View(img);
        }

        public ActionResult ViewImage(int id, string password)
        {
            ViewImageViewModel vm = new ViewImageViewModel();
            Image image = mgr.GetImage(id);
            if (image.Id == 0)
            {
                vm.IncorrectPassword = true;
            }

            if (Session["pw"] == null)
            {
                Session["pw"] = new List<int>();
            }

            List<int> images = (List<int>)Session["pw"];

            foreach (int i in images)
            {
                if (i == id)
                {
                    password = image.Password;
                }

            }

            //if (Session[$"pw-{id}"] != null)
            //{
            //    password = image.Password;
            //}

            vm.Image = image;

            if (password == null)
            {
                vm.Password = null;
                vm.IncorrectPassword = true;
            }
            else
            {
                if (password != image.Password)
                {
                    vm.Password = password;
                    vm.IncorrectPassword = true;
                }
                else
                {
                    vm.Password = password;
                    vm.IncorrectPassword = false;
                    vm.Image.Views = mgr.IncrementCount(id);
                    // Session[$"pw-{id}"] = password;   
                    images.Add(id);
                }

            }

            return View(vm);
        }


    }
}