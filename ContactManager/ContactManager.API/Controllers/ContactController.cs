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
        private readonly ILogger<ContactController> _logger;
        private readonly IContactRepository _contactRepository;
        public ContactController(ILogger<ContactController> logger, IContactRepository contactRepository)
        {
            _logger = logger;
            _contactRepository = contactRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Contact>> Get()
        {
            return await _contactRepository.Contacts();
        }

        [HttpPut("Contact/Update")]
        public async Task<RequestResult<Contact>> UpdateContact(Contact contact)
        {
            return await _contactRepository.UpsertContact(contact);
        }
    }
}
