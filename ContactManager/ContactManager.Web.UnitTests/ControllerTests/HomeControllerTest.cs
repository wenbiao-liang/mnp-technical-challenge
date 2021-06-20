using ContactManager.Web.Controllers;
using ContactManager.Web.Models;
using ContactManager.Web.Proxy;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Collections.Generic;

namespace ContactManager.Web.UnitTests
{
    [TestClass]
    public class HomeControllerTest
    {
        private Controller SetupController()
        {
            HttpClient httpClient = new HttpClient();
            AppSettings appSettings = new AppSettings();
            ApiService apiService = new ApiService(httpClient, appSettings);
            return new HomeController(apiService);
        }

        [TestMethod]
        public void TestIndexView()
        {
            // Arrange
            var controller = SetupController() as HomeController;

            // Act
            var result = controller.Index().Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResultBase));
            var viewResult = (ViewResultBase)result;
            Assert.AreEqual("Index", viewResult.ViewName);

            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(List<dtoContact>));
            var model = viewResult.Model as List<dtoContact>;
            Assert.IsNotNull(model);

            Assert.AreNotEqual(0, model.Count);
        }
        [TestMethod]
        public void TestHomeViewAbout()
        {
            // Arrange
            var controller = SetupController() as HomeController;

            // Act
            var result = controller.About();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResultBase));
            var viewResult = (ViewResultBase)result;
            Assert.AreEqual("About", viewResult.ViewName);

            Assert.IsNotNull(viewResult.ViewBag);
            Assert.IsNotNull(viewResult.ViewData["Message"]);
        }
        [TestMethod]
        public void TestHomeViewContact()
        {
            // Arrange
            var controller = SetupController() as HomeController;

            // Act
            var result = controller.Contact();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResultBase));
            var viewResult = (ViewResultBase)result;
            Assert.AreEqual("Contact", viewResult.ViewName);

            Assert.IsNotNull(viewResult.ViewBag);
            Assert.IsNotNull(viewResult.ViewData["Message"]);
        }
    }
}
