using ContactManager.Web.Models;
using ContactManager.Web.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ContactManager.Web.Controllers
{
    public class ContactController : Controller
    {
        private IApiService _contactService;
        public ContactController(IApiService contactService)
        {
            _contactService = contactService;
        }

        // ugly primitive cache for companies
        private static IEnumerable<SelectListItem> _companies = null;
        public async Task<IEnumerable<SelectListItem>> GetCompanies()
        {
            if (_companies == null)
            {
                var companies = await _contactService.GetCompanies(true);
                _companies = companies.Select(x => new SelectListItem() { Value = x.Name, Text = x.Name });
            }
            return _companies;
        }
        public async Task<ActionResult> New()
        {
            var model = new dtoContact()
            {
                Companies = await GetCompanies()
            };

            return View("New", model);
        }
        private async Task<ActionResult> Upsert(dtoContact contact, string viewName)
        {
            if (ModelState.IsValid)
            {
                var requestResult = await _contactService.UpdateContact(contact);
                if (requestResult.Result)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", requestResult.ErrorMessage);
                contact.Companies = await GetCompanies();
                return View(contact);
            }
            contact.Companies = await GetCompanies();
            return View(viewName, contact);
        }

        [HttpPost]
        public async Task<ActionResult> New(dtoContact contact)
        {
            return await Upsert(contact, "New");
        }

        public async Task<ActionResult> Edit(int Id)
        {
            var model = await _contactService.GetContactById(Id);

            if (model == null)
            {
                return RedirectToAction("New");
            }
            model.Companies = await GetCompanies();
            return View("Edit", model);
        }


        [HttpPost]
        public async Task<ActionResult> Edit(dtoContact contact)
        {
            return await Upsert(contact, "Edit");
        }
    }
}