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
        private static Random random = new Random((int)DateTime.Now.Ticks);
        public ContactRepositoryTest()
        {
            _contactRepositoryMock = new Mock<IContactRepository>() { CallBase = true };
            _loggerMock = new Mock<ILogger<ContactController>>() ;
        }
        private static string RandomString(int length)
        {
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[random.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }
        [Fact]
        public async void GetContactById_success()
        {
            // Arrange
            var fakeList = new List<Contact>() { new Contact() { Id = 1, Name = "fake" } }.AsEnumerable();
            _contactRepositoryMock.Setup(x => x.Contacts()).Returns(Task.FromResult(fakeList));

            // Act
            var contactController = new ContactController(
                _loggerMock.Object,
                _contactRepositoryMock.Object);
            var actionResult = await contactController.GetById(1);

            // Assert
            var contact = Assert.IsAssignableFrom<Contact>(actionResult);
            Assert.NotNull(contact);
            Assert.True(contact.Id == 1);
            Assert.True(contact.Name == "fake");
        }

        [Fact]
        public async void GetContactById_fail()
        {
            // Arrange
            var fakeList = new List<Contact>() { new Contact() { Id = 1, Name = "fake" } }.AsEnumerable();
            _contactRepositoryMock.Setup(x => x.Contacts()).Returns(Task.FromResult(fakeList));

            // Act
            var contactController = new ContactController(
                _loggerMock.Object,
                _contactRepositoryMock.Object);
            var contact = await contactController.GetById(2);

            // Assert
            Assert.Null(contact);
        }

        [Fact]
        public async void GetContacts_success()
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
        public async void GetEmptyContacts_success()
        {
            // Arrange, if database is empty
            IEnumerable<Contact> fakeList = null;
            _contactRepositoryMock.Setup(x => x.Contacts()).Returns(Task.FromResult(fakeList));

            // Act
            var contactController = new ContactController(
                _loggerMock.Object,
                _contactRepositoryMock.Object);
            var action = await contactController.Get();

            // Assert
            Assert.Null(action);
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
            Assert.True(result.Result);
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
        public async void Put_Insert_failure()
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
            var contact = new Contact() { Id = 0, Name = "fake", Title = "Fake", Company = "fake1", Address = "fake1", Email = "fake@fake.fake", Phone = "403-222-3333", LastDateContacted = new DateTime(2001, 01, 01) };
            var requestResult = await contactController.UpdateContact(contact);

            // Assert
            Assert.IsType<ActionResult<RequestResult<Contact>>>(requestResult);
            var result = Assert.IsAssignableFrom<RequestResult<Contact>>(requestResult.Value);

            Assert.NotNull(result);
            Assert.False(result.Result);
            Assert.Equal($"Contact with the same name already exists.", result.ErrorMessage);
        }

        [Fact]
        public async void Put_Insert_fail_validation()
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
            var contact = new Contact() { Id = 0, Name = "fak" };
            var requestResult = await contactController.UpdateContact(contact);

            // Assert
            Assert.IsType<ActionResult<RequestResult<Contact>>>(requestResult);
            var result = Assert.IsAssignableFrom<RequestResult<Contact>>(requestResult.Value);
            Assert.NotNull(result);
            Assert.False(result.Result); // name is too short
            Assert.NotNull(result.ErrorMessage);
            Assert.NotEqual(-1, result.ErrorMessage.IndexOf("title"));

            contact.Name = "fakest";
            requestResult = await contactController.UpdateContact(contact);
            Assert.IsType<ActionResult<RequestResult<Contact>>>(requestResult);
            result = Assert.IsAssignableFrom<RequestResult<Contact>>(requestResult.Value);
            Assert.NotNull(result);
            Assert.False(result.Result);    // missing title
            Assert.NotNull(result.ErrorMessage);
            Assert.NotEqual(-1, result.ErrorMessage.IndexOf("title"));

            contact.Title = "Fake";
            requestResult = await contactController.UpdateContact(contact);
            Assert.IsType<ActionResult<RequestResult<Contact>>>(requestResult);
            result = Assert.IsAssignableFrom<RequestResult<Contact>>(requestResult.Value);
            Assert.NotNull(result);
            Assert.False(result.Result);    // missing company
            Assert.NotNull(result.ErrorMessage);
            Assert.NotEqual(-1, result.ErrorMessage.IndexOf("company"));

            contact.Company = "HP";
            requestResult = await contactController.UpdateContact(contact);
            Assert.IsType<ActionResult<RequestResult<Contact>>>(requestResult);
            result = Assert.IsAssignableFrom<RequestResult<Contact>>(requestResult.Value);
            Assert.NotNull(result);
            Assert.True(result.Result);     // should be good now

            contact.Address = RandomString(101);
            requestResult = await contactController.UpdateContact(contact);
            Assert.IsType<ActionResult<RequestResult<Contact>>>(requestResult);
            result = Assert.IsAssignableFrom<RequestResult<Contact>>(requestResult.Value);
            Assert.NotNull(result);
            Assert.False(result.Result);     // should be bad now

            contact.Address = RandomString(100);
            contact.Email = RandomString(256);
            requestResult = await contactController.UpdateContact(contact);
            Assert.IsType<ActionResult<RequestResult<Contact>>>(requestResult);
            result = Assert.IsAssignableFrom<RequestResult<Contact>>(requestResult.Value);
            Assert.NotNull(result);
            Assert.False(result.Result);     // should be bad now

            contact.Email = RandomString(255);
            contact.Phone = RandomString(51);
            requestResult = await contactController.UpdateContact(contact);
            Assert.IsType<ActionResult<RequestResult<Contact>>>(requestResult);
            result = Assert.IsAssignableFrom<RequestResult<Contact>>(requestResult.Value);
            Assert.NotNull(result);
            Assert.False(result.Result);     // should be bad now

            contact.Phone = RandomString(50);
            contact.Comments = RandomString(513);
            requestResult = await contactController.UpdateContact(contact);
            Assert.IsType<ActionResult<RequestResult<Contact>>>(requestResult);
            result = Assert.IsAssignableFrom<RequestResult<Contact>>(requestResult.Value);
            Assert.NotNull(result);
            Assert.False(result.Result);     // should be bad now

            contact.Comments = RandomString(512);
            requestResult = await contactController.UpdateContact(contact);
            Assert.IsType<ActionResult<RequestResult<Contact>>>(requestResult);
            result = Assert.IsAssignableFrom<RequestResult<Contact>>(requestResult.Value);
            Assert.NotNull(result);
            Assert.True(result.Result);     // should be good now
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