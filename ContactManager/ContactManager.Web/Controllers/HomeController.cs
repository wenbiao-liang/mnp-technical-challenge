using ContactManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactManager.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new List<dtoContact>()
            {
                new dtoContact() { Id= 1, Name = "Wolter Disney", Title = "Founder & CEO", Company = "Disney", Phone = "444-444-5599", Address = "123 anywhere road", Email = "g.g@disney.com", LastDateContacted = new DateTime(2021,06, 22)},
                new dtoContact() { Id= 2, Name = "Mary Smith", Title= "VP Finance", Company = "HP", Phone = "433-544-5599", Address = "999 somewhere street NW", Email = "m.s@hp.com", LastDateContacted = new DateTime(2003,06, 22)},
            };
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}