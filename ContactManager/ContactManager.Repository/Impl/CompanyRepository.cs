using ContactManager.Repository.Interfaces;
using ContactManager.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Repository.Impl
{
    public class CompanyRepository : ICompanyRepository
    {
        // depending if I have time or not, we need to change these to a Database
        // need to query dbContext using EF or direct SP call through Dapper to get these.
        //
        // FOR NOW it "works", 
        private static List<Company> _fakeCompanies = new List<Company>()
        {
            new Company() { Id = 12, Name = "HP", Address = "Somewhere in Palo Alto?", IsActive = true },
            new Company() { Id = 23, Name = "Microsoft", Address = "Near Washington State", IsActive = true },
            new Company() { Id = 34, Name = "Disney", Address = "Beautiful sunshine Floridia", IsActive = true },
            new Company() { Id = 45, Name = "Blackberry", Address = "Cold Waterloo", IsActive = false },
        };
        public async Task<IEnumerable<Company>> Companies(bool isActive)
        {

            return await Task.Run(() => _fakeCompanies.Where(c => isActive ? c.IsActive == true : true));
        }
    }
}
