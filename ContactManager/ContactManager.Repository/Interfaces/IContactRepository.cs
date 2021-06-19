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
        Task<IEnumerable<Company>> Companies(bool isActive);
        Task<IEnumerable<Contact>> Contacts();
    }
}
