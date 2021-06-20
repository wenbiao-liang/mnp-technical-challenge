using ContactManager.Web.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ContactManager.Web.Proxy
{
    /// <summary>
    /// a combined service to call API through different controllers, is probably suficient for small program like this
    /// </summary>
    public class ApiService : IApiService
    {
        private readonly IAppSettings _appSettings;
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient, IAppSettings appSettings)
        {
            _httpClient = httpClient;
            _appSettings = appSettings;
        }

        public async Task<List<dtoCompany>> GetCompanies(bool isActive)
        {
            var uri = $"{_appSettings.CompanyUrl}?isActive={isActive}";
            //_httpClient.BaseAddress = new System.Uri(uri);
            var response = await _httpClient.GetAsync(uri);
            var responseStr = await response.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(responseStr) ?
                new List<dtoCompany>() :
                JsonConvert.DeserializeObject<List<dtoCompany>>(responseStr);
        }

        public async Task<dtoContact> GetContactById(int Id)
        {
            var uri = $"{_appSettings.ContactUrl}/ID/{Id}";
            _httpClient.BaseAddress = new System.Uri(uri);
            var response = await _httpClient.GetAsync(uri);
            var responseStr = await response.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(responseStr) ?
                null :
                JsonConvert.DeserializeObject<dtoContact>(responseStr);
        }
        public async Task<List<dtoContact>> GetContacts()
        {
            var uri = $"{_appSettings.ContactUrl}";
            _httpClient.BaseAddress = new System.Uri(uri);
            var response = await _httpClient.GetAsync(uri);
            var responseStr = await response.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(responseStr) ?
                new List<dtoContact>() :
                JsonConvert.DeserializeObject<List<dtoContact>>(responseStr);
        }
        public async Task<RequestResult<dtoContact>> UpdateContact(dtoContact contact)
        {
            var uri = $"{_appSettings.ContactUrl}/Update";
            var content = new StringContent(JsonConvert.SerializeObject(contact), UTF8Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(uri, content);
            var responseStr = await response.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(responseStr) ?
                null :
                JsonConvert.DeserializeObject<RequestResult<dtoContact>>(responseStr);
        }
    }
}