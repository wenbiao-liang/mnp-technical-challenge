using ContactManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Web.Proxy
{
    /// <summary>
    /// a combined service to call API through different controllers, is probably suficient for small program like this
    /// </summary>
    public interface IApiService
    {
        Task<List<dtoCompany>> GetCompanies(bool isActive);
        Task<dtoContact> GetContactById(int Id);
        Task<List<dtoContact>> GetContacts();
        Task<RequestResult<dtoContact>> UpdateContact(dtoContact contact);
    }
}
