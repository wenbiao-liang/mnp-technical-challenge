using ContactManager.Repository.Interfaces;
using ContactManager.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            var result = await _contactRepository.Contacts();
            return result;
        }
        [HttpGet("ID/{Id}")]
        public async Task<Contact> GetById(int Id)
        {
            return await _contactRepository.GetContactById(Id);
        }

        [HttpPut("Update")]
        public async Task<ActionResult<RequestResult<Contact>>> UpdateContact(Contact contact)
        {
            var result = true;
            var errorMessage = string.Empty;
            if (contact == null || !contact.IsValid(out errorMessage))
            {
                result = false;
            }
            else
            {
                // with dbContext, all these are not necessary
                var fakeContacts = await _contactRepository.GetContacts();
                var existing = await Task.Run(() => fakeContacts.FirstOrDefault(c => c.Id == contact.Id));
                if (contact.Id == 0 || existing == null)
                {
                    // simulate one business fail case
                    if (fakeContacts.Any(c => c.Name == contact.Name))
                    {
                        result = false;
                        errorMessage = $"Contact with the same name already exists.";
                    }
                    else
                    {
                        // insert, pretend async
                        contact = await _contactRepository.InsertContact(contact);
                    }
                }
                else
                {
                    // update
                    contact = await _contactRepository.UpdateContact(contact);
                }
            }

            return new RequestResult<Contact>() { Data = contact, Result = result, ErrorMessage = errorMessage };
        }
    }
}
