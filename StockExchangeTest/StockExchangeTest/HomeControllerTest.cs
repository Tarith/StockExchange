using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockExchangeClient.Controllers;
using StockExchangeClient.Models;
using System.Linq;
using System.Web.Mvc;
using System.Security.Principal;
using System.Threading;
using Moq;
using System.Collections.Generic;

namespace StockExchangeTest
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Home_IndexReturnsTheRightView()
        {
            var controller = new HomeController(new MockDataBase(), new MockService());
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Home_EnforceAuthenticatedUsers()
        {
            var controller = new HomeController(new MockDataBase(), new MockService());
            var type = controller.GetType();
            var attributes = type.GetCustomAttributes(typeof(AuthorizeAttribute), true);
            Assert.IsTrue(attributes.Any());
        }

        [TestMethod]
        public void Home_GetTickerReturnsTheRightView()
        {
            var listofStocks = CreateListStock();
            var controller = CreateHomeControllerWithDataForUser("unittestuser@test.test", listofStocks);

            var result = controller.GetTicker() as PartialViewResult;

            Assert.AreEqual("_Ticker", result.ViewName);
        }

        [TestMethod]
        public void Home_GetTickerGetsTickersForCurrentUser()
        {
            var listofStocks = CreateListStock();
            var controller = CreateHomeControllerWithDataForUser("unittestuser@test.test", listofStocks);

            var result = controller.GetTicker() as PartialViewResult;
            var model = (List<StockManagerVM>)result.Model;

            Assert.AreEqual(1, model.Count());
        }

        [TestMethod]
        public void Home_GetTickerPassessTheRightModel()
        {
            var listofStocks = CreateListStock();
            var controller = CreateHomeControllerWithDataForUser("unittestuser@test.test", listofStocks);

            var result = controller.GetTicker() as PartialViewResult;

            Assert.IsInstanceOfType(result.Model, typeof(List<StockManagerVM>));
        }

        [TestMethod]
        public void Home_AddDuplicateStockReturnsErrorMessage()
        {
            var listofStocks = CreateListStock();
            var stock = CreateStockObject();
            var controller = CreateHomeControllerWithDataForUser("unittestuser@test.test", listofStocks);

            var result = controller.AddStock(stock) as JsonResult;

            Assert.AreEqual("{ success = False, message = The code already exists for this user }", result.Data.ToString());
        }

        [TestMethod]
        public void Home_AddStockReturnsSuccess()
        {
            var listofStocks = CreateListStock();
            var stock = CreateStockObject();
            var controller = CreateHomeControllerWithoutDataForUser("unittestuser@test.test");

            var result = controller.AddStock(stock) as JsonResult;

            Assert.IsTrue(result.Data.ToString().Contains("success = True"));
        }

        [TestMethod]
        public void Home_DeleteStockReturnsSuccess()
        {
            var listofStocks = CreateListStock();
            var controller = CreateHomeControllerWithDataForUser("unittestuser@test.test", listofStocks);

            var result = controller.DeleteStock(1) as JsonResult;

            Assert.IsTrue(result.Data.ToString().Contains("success = True"));
        }

        private HomeController CreateHomeControllerWithDataForUser(string username, List<Stock> stockList)
        {
            var mock = new Mock<ControllerContext>();
            mock.SetupGet(p => p.HttpContext.User.Identity.Name).Returns(username);
            mock.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            var controller = new HomeController(new MockDataBase(stockList), new MockService());
            controller.ControllerContext = mock.Object;

            return controller;
        }
        private HomeController CreateHomeControllerWithoutDataForUser(string username)
        {
            var mock = new Mock<ControllerContext>();
            mock.SetupGet(p => p.HttpContext.User.Identity.Name).Returns(username);
            mock.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            var controller = new HomeController(new MockDataBase(), new MockService());
            controller.ControllerContext = mock.Object;

            return controller;
        }

        private HomeController CreateHomeControllerWithoutUser()
        {
            var mock = new Mock<ControllerContext>();
            mock.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(false);

            var controller = new HomeController(new MockDataBase(), new MockService());
            controller.ControllerContext = mock.Object;

            return controller;
        }

        private AddStockVM CreateStockObject()
        {
            return new AddStockVM
            {
                Code = "TESTCODE"
            };
        }

        private List<Stock> CreateListStock()
        {
            return new List<Stock>
            {
               new Stock
               {
                   Id = 1,
                   Code = "TESTCODE",
                   UserName = "unittestuser@test.test"
               }
            };
        }
    }
}
