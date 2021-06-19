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
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly ICompanyRepository _companyRepository;
        public CompanyController(ILogger<CompanyController> logger, ICompanyRepository companyRepository)
        {
            _logger = logger;
            _companyRepository = companyRepository;
        }
        [HttpGet("Companies")]
        public async Task<IEnumerable<Company>> GetCompanies(bool isActive)
        {
            return await _companyRepository.Companies(isActive);
        }
    }
}
