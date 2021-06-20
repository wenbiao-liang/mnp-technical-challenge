using ContactManager.Web.Controllers;
using ContactManager.Web.Models;
using ContactManager.Web.Proxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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

        [TestMethod]
        public void TestGetNewView()
        {
            var controller = SetupController() as ContactController;

            var result = controller.New().Result;

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
        public async Task TestGetEditView()
        {
            var controller = SetupController() as ContactController;

            var redirectResult = controller.Edit(0).Result as RedirectToRouteResult;
            Assert.AreEqual("New", redirectResult.RouteValues["action"]);

            var result = await controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResultBase));
            var viewResult = (ViewResultBase)result;
            Assert.AreEqual("Edit", viewResult.ViewName);

            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(dtoContact));
            var model = viewResult.Model as dtoContact;
            Assert.IsNotNull(model);

            Assert.AreEqual(3, model.Companies.Count());    // Can I be so sure?
        }
    }
}