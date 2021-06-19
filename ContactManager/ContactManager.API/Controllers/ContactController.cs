using ContactManager.Repository.Interfaces;
using ContactManager.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ContactController> _logger;
        private readonly IContactRepository _contactRepository;
        private readonly ICompanyRepository _companyRepository;
        public ContactController(ILogger<ContactController> logger, IContactRepository contactRepository, ICompanyRepository companyRepository)
        {
            _logger = logger;
            _contactRepository = contactRepository;
            _companyRepository = companyRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Contact>> Get()
        {
            return await _contactRepository.Contacts();
        }

        [HttpGet("Companies")]
        public async Task<IEnumerable<Company>> GetCompanies(bool isActive)
        {
            return await _companyRepository.Companies(isActive);
        }
        [HttpPut("Contact/Update")]
        public async Task<RequestResult<Contact>> UpdateContact(Contact contact)
        {
            return await _contactRepository.UpsertContact(contact);
        }
    }
}
