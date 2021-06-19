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
        // remove after porting to use DB
        public virtual async Task<List<Contact>> GetContacts()
        {
            return await Task.Run(() => _fakeContacts);
        }

        //
        private readonly ILogger<ContactRepository> _logger;
        public ContactRepository(ILogger<ContactRepository> logger)
        {
            _logger = logger;
        }

        public virtual async Task<IEnumerable<Contact>> Contacts()
        {
            return await Task.Run(() => GetContacts());
        }

        public virtual async Task<Contact> InsertContact(Contact contact)
        {
            var contacts = await GetContacts();
            // create a fakeId, not good for multiple insert?
            contact.Id = contacts.Max(c => c.Id) + 1;

            await Task.Run(() => contacts.Add(contact));

            // in real db call, DB may add more attributes
            return contact;
        }
        public virtual async Task<Contact> UpdateContact(Contact contact)
        {
            var contacts = await GetContacts();
            var existing = contacts.FirstOrDefault(x => x.Id == contact.Id);
            if (existing != null)
            {
                // Todo: replace this with db call
                contact = await Task.Run(() => (Contact)contact.Clone());
            }
            else
            {
                contact = await InsertContact(contact);
            }

            return contact;
        }
    }
}
