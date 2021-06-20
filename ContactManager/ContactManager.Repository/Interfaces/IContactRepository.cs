using ContactManager.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Repository.Interfaces
{
    public interface IContactRepository
    {
        // Temp function to fake DbContext
        Task<List<Contact>> GetContacts();

        //
        Task<IEnumerable<Contact>> Contacts();
        Task<Contact> InsertContact(Contact contact);
        Task<Contact> UpdateContact(Contact contact);
    }
}
