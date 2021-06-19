using ContactManager.Repository.Interfaces;
using ContactManager.Repository.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly ILogger<ContactRepository> _logger;
        public ContactRepository(ILogger<ContactRepository> logger)
        {
            _logger = logger;
        }
        public async Task<IEnumerable<Company>> Companies(bool isActive)
        {
            // need to query dbContext using EF or direct SP call through Dapper, depending if I have time or not
            var fakeCompanies = new List<Company>()
            {
                new Company() { Id = 12, Name = "HP", Address = "Somewhere in Palo Alto?", IsActive = true },
                new Company() { Id = 23, Name = "Microsoft", Address = "Near Washington State", IsActive = true },
                new Company() { Id = 34, Name = "Disney", Address = "Beautiful sunshine Floridia", IsActive = true },
                new Company() { Id = 45, Name = "Blackberry", Address = "Cold Waterloo", IsActive = false },
            };

            return await Task.Run(() => fakeCompanies.Where(c => isActive ? c.IsActive == true : true));
        }

        public async Task<IEnumerable<Contact>> Contacts()
        {
            var fakeContacts = new List<Contact>()
            {
                new Contact() { Id= 1, Name = "Wolter Disney", Title = "Founder & CEO", Company = "Disney", Phone = "444-444-5599", Address = "123 anywhere road", Email = "g.g@disney.com", LastDateContacted = new DateTime(2021,06, 22)},
                new Contact() { Id= 2, Name = "Mary Smith", Title= "VP Finance", Company = "HP", Phone = "433-544-5599", Address = "999 somewhere street NW", Email = "m.s@hp.com", LastDateContacted = new DateTime(2003,06, 22)},
            };
            return await Task.Run(() => fakeContacts);
        }
    }
}
