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
            var controller = SetupController() as HomeController;

            var result = controller.Index().Result;

            Assert.IsInstanceOfType(result, typeof(ViewResultBase));
            var viewResult = (ViewResultBase)result;
            Assert.AreEqual("Index", viewResult.ViewName);

            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(List<dtoContact>));
            var model = viewResult.Model as List<dtoContact>;
            Assert.IsNotNull(model);

            Assert.AreEqual(2, model.Count);    // Can I be so sure?
        }
        [TestMethod]
        public void TestHomeViewAbout()
        {
            var controller = SetupController() as HomeController;

            var result = controller.About();

            Assert.IsInstanceOfType(result, typeof(ViewResultBase));
            var viewResult = (ViewResultBase)result;
            Assert.AreEqual("About", viewResult.ViewName);

            Assert.IsNotNull(viewResult.ViewBag);
            Assert.IsNotNull(viewResult.ViewData["Message"]);
        }
        [TestMethod]
        public void TestHomeViewContact()
        {
            var controller = SetupController() as HomeController;

            var result = controller.Contact();

            Assert.IsInstanceOfType(result, typeof(ViewResultBase));
            var viewResult = (ViewResultBase)result;
            Assert.AreEqual("Contact", viewResult.ViewName);

            Assert.IsNotNull(viewResult.ViewBag);
            Assert.IsNotNull(viewResult.ViewData["Message"]);
        }
    }
}
