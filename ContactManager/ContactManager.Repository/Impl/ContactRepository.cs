using ContactManager.Repository.Interfaces;
using ContactManager.Repository.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.Repository.Impl
{
    public class ContactRepository : IContactRepository
    {
        // depending if I have time or not, we need to change these to a Database
        // need to query dbContext using EF or direct SP call through Dapper to get these.
        //
        // FOR NOW it "works", 
        private static List<Contact> _fakeContacts = new List<Contact>()
        {
            new Contact() { Id= 1, Name = "Wolter Disney", Title = "Founder & CEO", Company = "Disney", Phone = "444-444-5599", Address = "123 anywhere road", Email = "g.g@disney.com", LastDateContacted = new DateTime(2021,06, 22)},
            new Contact() { Id= 2, Name = "Mary Smith", Title= "VP Finance", Company = "HP", Phone = "433-544-5599", Address = "999 somewhere street NW", Email = "m.s@hp.com", LastDateContacted = new DateTime(2003,06, 22)},
        };

        private readonly ILogger<ContactRepository> _logger;
        public ContactRepository(ILogger<ContactRepository> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Contact>> Contacts()
        {
            return await Task.Run(() => _fakeContacts);
        }

        public async Task<RequestResult<Contact>> UpsertContact(Contact contact)
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
                var existing = await Task.Run(() => _fakeContacts.FirstOrDefault(c => c.Id == contact.Id));
                if (contact.Id == 0 || existing == null)
                {
                    // create a fakeId, not good for multiple insert?
                    contact.Id = _fakeContacts.Max(c => c.Id) + 1;

                    // simulate one business fail case
                    if (_fakeContacts.Any(c => c.Name == contact.Name))
                    {
                        result = false;
                        errorMessage = $"Contact with the same name already exists.";
                    }
                    else
                    {
                        // insert, pretend async
                        await Task.Run(() => _fakeContacts.Add(contact));
                    }
                }
                else
                {
                    // update, pretend async
                    existing = await Task.Run(() => (Contact)contact.Clone());
                }
            }

            return new RequestResult<Contact>() { Data = contact, Result = result, ErrorMessage = errorMessage };
        }
    }
}
