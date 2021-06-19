using ContactManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactManager.Web.Controllers
{
    public class ContactController : Controller
    {
        // this list should be taken from API
        private List<SelectListItem> companies = new List<SelectListItem>()
        {
            new SelectListItem () { Value ="HP", Text = "HP"},
            new SelectListItem () { Value ="Microsoft", Text = "Microsoft"},
            new SelectListItem () { Value ="Disney", Text = "Disney"},
        };
        public ActionResult New()
        {
            var model = new dtoContact()
            {
                Id = 0,
                Name = "",
                Title = "",
                Company = "",
                Phone = "",
                Address = "",
                Email = "",
                LastDateContacted = DateTime.Today,
                Companies = companies
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult New(dtoContact contact)
        {
            if (ModelState.IsValid)
            {
                var nameAlreadyExists = "AAAAA".Equals(contact.Name);
                // update db
                if (nameAlreadyExists)
                {
                    //fake adding error message to ModelState
                    ModelState.AddModelError("Name", "Contact Name Already Exists.");

                    contact.Companies = companies;
                    return View(contact);
                }
                return RedirectToAction("Index", "Home");
            }
            contact.Companies = companies;
            return View(contact);
        }

        public ActionResult Edit(int ID)
        {
            var model = new dtoContact()
            {
                Id = ID,
                Name = "",
                Title = "",
                Company = "",
                Phone = "",
                Address = "",
                Email = "",
                LastDateContacted = DateTime.Today,
                Companies = companies
            };

            return View(model);
        }
    }
}