using ContactManager.API.Controllers;
using ContactManager.Repository.Interfaces;
using ContactManager.Repository.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContactManager.UnitTests.RepositoryTests
{
    public class CompanyControllerTest
    {
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly Mock<ILogger<CompanyController>> _loggerMock;
        public CompanyControllerTest()
        {
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _loggerMock = new Mock<ILogger<CompanyController>>();
        }
        [Fact]
        public async void GetCompanies_success()
        {
            // Arrange
            var fakeList = new List<Company>() { 
                new Company() { Id = 22, Name = "fake", IsActive = true },
                new Company() { Id = 1, Name = "more fake", IsActive = true },
                new Company() { Id = 333, Name = "still fake", IsActive = false }
            }.AsEnumerable();

            _companyRepositoryMock
                .Setup(x => x.Companies(It.IsAny<bool>()))
                .Returns((bool isActive) => { return Task.FromResult(fakeList.Where(c => isActive ? c.IsActive == true : true)); });

            // Act
            var companyController = new CompanyController(
                _loggerMock.Object,
                _companyRepositoryMock.Object);
            var actionResult0 = await companyController.Get(true);
            var actionResult1 = await companyController.Get(false);

            // Assert
            var result = Assert.IsAssignableFrom<IEnumerable<Company>>(actionResult0);
            Assert.Equal(2, result.Count());
            var company = result.FirstOrDefault();
            Assert.NotNull(company);
            Assert.True(company.Id == 22);
            Assert.True(company.Name == "fake");
            company = result.LastOrDefault();
            Assert.NotNull(company);
            Assert.True(company.Id == 1);
            Assert.True(company.Name == "more fake");

            // Assert
            result = Assert.IsAssignableFrom<IEnumerable<Company>>(actionResult1);
            Assert.Equal(3, result.Count());
            company = result.LastOrDefault();
            Assert.NotNull(company);
            Assert.True(company.Id == 333);
            Assert.True(company.Name == "still fake");
        }
    }
}