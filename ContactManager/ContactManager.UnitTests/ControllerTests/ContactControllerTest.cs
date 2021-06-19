using ContactManager.Repository.Impl;
using ContactManager.Repository.Interfaces;
using ContactManager.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using ContactManager.API.Controllers;

namespace ContactManager.UnitTests
{
    public class ContactRepositoryTest
    {
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly Mock<ILogger<ContactController>> _loggerMock;
        public ContactRepositoryTest()
        {
            _contactRepositoryMock = new Mock<IContactRepository>() { CallBase = true };
            _loggerMock = new Mock<ILogger<ContactController>>() ;
        }
        [Fact]
        public async void Get_Contacts_success()
        {
            // Arrange
            var fakeList = new List<Contact>() { new Contact() { Id = 1, Name = "fake" } }.AsEnumerable();
            _contactRepositoryMock.Setup(x => x.Contacts()).Returns(Task.FromResult(fakeList));

            // Act
            var contactController = new ContactController(
                _loggerMock.Object,
                _contactRepositoryMock.Object);
            var actionResult = await contactController.Get();

            // Assert
            var result = Assert.IsAssignableFrom<IEnumerable<Contact>>(actionResult);
            var resultList = result.ToList();
            Assert.Single(resultList);
            var contact0 = result.FirstOrDefault();
            Assert.NotNull(contact0);
            Assert.True(contact0.Id == 1);
            Assert.True(contact0.Name == "fake");
        }

        [Fact]
        public async void Put_Insert_success()
        {
            // Arrange
            var fakeList = new List<Contact>() { 
                new Contact() { Id = 1, Name = "fake" } ,
                new Contact() { Id = 2, Name = "more fake" } ,
                new Contact() { Id = 3, Name = "still fake" } ,
            };
            _contactRepositoryMock.Setup(x => x.GetContacts()).Returns(Task.FromResult(fakeList));
            _contactRepositoryMock.Setup(x => x.Contacts()).Returns(Task.FromResult(fakeList.AsEnumerable()));
            _contactRepositoryMock.Setup(x => x.InsertContact(It.IsAny<Contact>()))
                .Callback((Contact c) => {
                    c.Id = fakeList.Max(c => c.Id) + 1;
                    fakeList.Add(c); 
                })
                .ReturnsAsync((Contact c) => c);

            // Act
            var contactController = new ContactController(
                _loggerMock.Object,
                _contactRepositoryMock.Object);
            // Act: insert
            var contact = new Contact() { Id = 0, Name = "fake1", Title = "Fake", Company = "fake1", Address = "fake1", Email = "fake@fake.fake", Phone = "403-222-3333", LastDateContacted = new DateTime(2001, 01, 01) };
            var requestResult = await contactController.UpdateContact(contact);

            // Assert
            Assert.IsType<ActionResult<RequestResult<Contact>>>(requestResult);
            var result = Assert.IsAssignableFrom<RequestResult<Contact>>(requestResult.Value);

            Assert.NotNull(result);
            var contact0 = result.Data;
            Assert.True(contact0.Id == 4);
            Assert.True(contact0.Name == "fake1");

            // 
            var actionResult1 = await contactController.Get();
            var results = Assert.IsAssignableFrom<IEnumerable<Contact>>(actionResult1);
            Assert.Equal(4, results.Count());
            var contact1 = results.FirstOrDefault(x=>x.Id == contact0.Id);
            Assert.NotNull(contact1);
            Assert.Equal("fake1", contact1.Name);
        }

        [Fact]
        public async void Put_Update_success()
        {
            // Arrange
            var fakeList = new List<Contact>() {
                new Contact() { Id = 1, Name = "fake" } ,
                new Contact() { Id = 2, Name = "more fake" } ,
                new Contact() { Id = 3, Name = "still fake" } ,
            };
            _contactRepositoryMock.Setup(x => x.GetContacts()).Returns(Task.FromResult(fakeList));
            _contactRepositoryMock.Setup(x => x.Contacts()).Returns(Task.FromResult(fakeList.AsEnumerable()));
            _contactRepositoryMock.Setup(x => x.UpdateContact(It.IsAny<Contact>()))
                .Callback((Contact c) => {
                    var ec = fakeList.FirstOrDefault(x => x.Id == c.Id);
                    fakeList.Remove(ec);
                    fakeList.Add(c);
                })
                .ReturnsAsync((Contact c) => c);

            // Act
            var contactController = new ContactController(
                _loggerMock.Object,
                _contactRepositoryMock.Object);
            // Act: insert
            var contact = new Contact() { Id = 1, Name = "fake1", Title = "Fake", Company = "fake1", Address = "fake1", Email = "fake@fake.fake", Phone = "403-222-3333", LastDateContacted = new DateTime(2001, 01, 01) };
            var requestResult = await contactController.UpdateContact(contact);

            // Assert
            Assert.IsType<ActionResult<RequestResult<Contact>>>(requestResult);
            var result = Assert.IsAssignableFrom<RequestResult<Contact>>(requestResult.Value);

            Assert.NotNull(result);
            var contact0 = result.Data;
            Assert.Equal(1, contact0.Id);
            Assert.Equal("fake1", contact0.Name);

            // 
            var actionResult1 = await contactController.Get();
            var results = Assert.IsAssignableFrom<IEnumerable<Contact>>(actionResult1);
            Assert.Equal(3, results.Count());
            var contact1 = results.FirstOrDefault(x => x.Id == contact0.Id);
            Assert.NotNull(contact1);
            Assert.Equal("fake1", contact1.Name);
            Assert.Equal("fake@fake.fake", contact1.Email);
        }
    }
}