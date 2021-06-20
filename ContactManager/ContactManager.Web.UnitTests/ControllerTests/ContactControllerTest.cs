using ContactManager.Web.Controllers;
using ContactManager.Web.Models;
using ContactManager.Web.Proxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ContactManager.Web.UnitTests.ControllerTests
{
    [TestClass]
    public class ContactControllerTest
    {
        private Controller SetupController()
        {
            HttpClient httpClient = new HttpClient();
            AppSettings appSettings = new AppSettings();
            ApiService apiService = new ApiService(httpClient, appSettings);
            return new ContactController(apiService);
        }
        // should have a utility dll to contain these helper functions
        private static Random random = new Random((int)DateTime.Now.Ticks);
        private static string RandomString(int length)
        {
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[random.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }

        [TestMethod]
        public void TestGetNewView()
        {
            // Arrange
            var controller = SetupController() as ContactController;

            // Act
            var result = controller.New().Result;

            // Assert, should return a brand new contact model from the controller
            Assert.IsInstanceOfType(result, typeof(ViewResultBase));
            var viewResult = (ViewResultBase)result;
            Assert.AreEqual("New", viewResult.ViewName);

            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(dtoContact));
            var model = viewResult.Model as dtoContact;
            Assert.IsNotNull(model);

            Assert.AreEqual(3, model.Companies.Count());    // Can I be so sure?
        }

        [TestMethod]
        public async Task TestGetEditView_Redirect()
        {
            // Arrange
            var controller = SetupController() as ContactController;

            // Act
            var redirectResult = controller.Edit(0).Result as RedirectToRouteResult;

            // Assert, Id=0 should not exist, expect redirect to New view
            Assert.AreEqual("New", redirectResult.RouteValues["action"]);
        }
        [TestMethod]
        public async Task TestGetEditView()
        {
            // Arrange
            var controller = SetupController() as ContactController;

            // Act, Id=1 should exist
            var result = await controller.Edit(1);

            // Assert, should return Edit view and correct model
            Assert.IsInstanceOfType(result, typeof(ViewResultBase));
            var viewResult = (ViewResultBase)result;
            Assert.AreEqual("Edit", viewResult.ViewName);

            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(dtoContact));
            var model = viewResult.Model as dtoContact;
            Assert.IsNotNull(model);

            Assert.AreEqual(3, model.Companies.Count());    // Can I be so sure?
        }

        // need to test 3 things:
        // 1. model validations
        // 2. controller behaviour if model is valid
        // 3. controller behaviour if model is NOT valid
        [TestMethod]
        public void TestPutNewView_ValidateModel_Fail()
        {
            // Arrange
            var contact = new dtoContact()
            {
                Id = 0,
                Name = "Fake"
            };
            var context  = new ValidationContext(contact, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isModelStateValid = Validator.TryValidateObject(contact, context, results, true);

            Assert.IsFalse(isModelStateValid);
            Assert.AreNotEqual(0, results.Count());
            Assert.IsTrue(results.Any(x => x.ErrorMessage.IndexOf("name") != -1));      // must have error about name is bad
            Assert.IsTrue(results.Any(x => x.ErrorMessage.IndexOf("Title") != -1));     // must have error about title is bad
            Assert.IsTrue(results.Any(x => x.ErrorMessage.IndexOf("Company") != -1));   // must have error about company is bad
            Assert.IsTrue(results.Any(x => x.ErrorMessage.IndexOf("Phone") != -1));     // must have error about phone is bad
            Assert.IsTrue(results.Any(x => x.ErrorMessage.IndexOf("Email") != -1));     // must have error about email is bad
        }

        [TestMethod]
        public void TestPutNewView_ValidateModel_Success()
        {
            // Arrange
            var contact = new dtoContact()
            {
                Id = 0,
                Name = "FakeName",
                Title = "FakeTitle",
                Company = "FakeCompany",
                Email = "Fake@Company.COM",
                Phone = "4032223333"
            };
            var context = new ValidationContext(contact, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isModelStateValid = Validator.TryValidateObject(contact, context, results, true);

            // Arrange: model should be good
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestPutNewView_ModelState_NotValid()
        {
            // Arrange
            var controller = SetupController() as ContactController;
            var contact = new dtoContact()
            {
                Id = 0,
                Name = "Fake"
            };

            // Act, any model error should be fine
            controller.ModelState.AddModelError("Name", "Fake Name Error");
            var result = controller.New(contact).Result;

            // Assert, should remain in New view with at least one Error 
            Assert.IsInstanceOfType(result, typeof(ViewResultBase));
            var viewResult = (ViewResultBase)result;
            Assert.AreEqual("New", viewResult.ViewName);

            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(dtoContact));
            var model = viewResult.Model as dtoContact;
            Assert.IsNotNull(model);

            Assert.AreEqual(3, model.Companies.Count());    // Can I be so sure?
        }

        [TestMethod]
        public void TestPutNewView_ModelState_IsValid_InsertFails()
        {
            // Arrange
            var controller = SetupController() as ContactController;
            var contact = new dtoContact()
            {
                Id = 0,
                Name = "F"
            };

            // ModelState is valid, however, the backend should reject the model
            controller.ModelState.Clear();

            // Act
            var result = controller.New(contact).Result;

            // Assert, should remain in New page with error displayed
            Assert.IsInstanceOfType(result, typeof(ViewResultBase));
            var viewResult = (ViewResultBase)result;
            Assert.AreEqual("New", viewResult.ViewName);

            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(dtoContact));
            var model = viewResult.Model as dtoContact;
            Assert.IsNotNull(model);

            Assert.AreEqual(3, model.Companies.Count());

            Assert.AreEqual(1, controller.ModelState.Keys.Count);
            var key = controller.ModelState.Keys.FirstOrDefault();
            var errors = controller.ModelState[key].Errors;
            Assert.AreNotEqual(0, errors.Count);
            // should have 
            Assert.AreNotEqual(-1, errors.FirstOrDefault().ErrorMessage.IndexOf("name"));
        }

        [TestMethod]
        public void TestPutNewView_ModelState_IsValid_InsertSuccess()
        {
            // Arrange
            var controller = SetupController() as ContactController;
            var contact = new dtoContact()
            {
                Id = 0,
                Name = RandomString(36),
                Title = RandomString(5),
                Company = RandomString(10),
            };

            // ModelState is valid,  model is also good, the backend should accept the model
            controller.ModelState.Clear();

            // Act
            var result = controller.New(contact).Result;

            // Assert, once updated into database, we should redirect to Home/Index
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            var redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
        }

        // Edit tests
        [TestMethod]
        public void TestPutEditView_ModelState_NotValid()
        {
            // Arrange
            var controller = SetupController() as ContactController;
            var contact = new dtoContact()
            {
                Id = 123,
                Name = "Fake"
            };

            // Act, any model error should be fine
            controller.ModelState.AddModelError("Name", "Fake Name Error");
            var result = controller.Edit(contact).Result;

            // Assert, should remain in New view with at least one Error 
            Assert.IsInstanceOfType(result, typeof(ViewResultBase));
            var viewResult = (ViewResultBase)result;
            Assert.AreEqual("Edit", viewResult.ViewName);

            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(dtoContact));
            var model = viewResult.Model as dtoContact;
            Assert.IsNotNull(model);

            Assert.AreEqual(3, model.Companies.Count());    // Can I be so sure?
        }

        [TestMethod]
        public void TestPutEditView_ModelState_IsValid_UpdateFails()
        {
            // Arrange
            var controller = SetupController() as ContactController;
            var contact = new dtoContact()
            {
                Id = 1,
                Name = "F"
            };

            // ModelState is valid, however, the backend should reject the model
            controller.ModelState.Clear();

            // Act
            var result = controller.Edit(contact).Result;

            // Assert, should remain in New page with error displayed
            Assert.IsInstanceOfType(result, typeof(ViewResultBase));
            var viewResult = (ViewResultBase)result;
            Assert.AreEqual("Edit", viewResult.ViewName);

            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(dtoContact));
            var model = viewResult.Model as dtoContact;
            Assert.IsNotNull(model);

            Assert.AreEqual(3, model.Companies.Count());

            Assert.AreEqual(1, controller.ModelState.Keys.Count);
            var key = controller.ModelState.Keys.FirstOrDefault();
            var errors = controller.ModelState[key].Errors;
            Assert.AreNotEqual(0, errors.Count);
            // should have 
            Assert.AreNotEqual(-1, errors.FirstOrDefault().ErrorMessage.IndexOf("name"));
        }

        [TestMethod]
        public void TestPutEditView_ModelState_IsValid_UpdateSuccess()
        {
            // Arrange
            var controller = SetupController() as ContactController;
            var contact = new dtoContact()
            {
                Id = 1,
                Name = RandomString(36),
                Title = RandomString(5),
                Company = RandomString(10),
            };

            // ModelState is valid,  model is also good, the backend should accept the model
            controller.ModelState.Clear();

            // Act
            var result = controller.Edit(contact).Result;

            // Assert, once updated into database, we should redirect to Home/Index
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            var redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
        }
    }
}