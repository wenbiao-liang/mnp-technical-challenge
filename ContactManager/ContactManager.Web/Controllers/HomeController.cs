using ContactManager.Web.Models;
using ContactManager.Web.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ContactManager.Web.Controllers
{
    public class HomeController : Controller
    {
        private IApiService _contactService;
        public HomeController(IApiService contactService)
        {
            _contactService = contactService;
        }

        public async Task<ActionResult> Index()
        {
            var model = await _contactService.GetContacts();
            return View("Index", model.OrderBy(c => c.Name).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "MNP Contact Manager page.";

            return View("About");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact MNP at 403-222-3333.";

            return View("Contact");
        }
    }
}