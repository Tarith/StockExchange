using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockExchangeClient.Controllers;
using StockExchangeClient.Models;
using System.Linq;
using System.Web.Mvc;
using System.Security.Principal;
using System.Threading;

namespace StockExchangeTest
{
    [TestClass]
    public class ErrorControllerTest
    {
        [TestMethod]
        public void Error_IndexReturnsTheRightView()
        {
            var controller = new ErrorController();
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Error", result.ViewName);
        }
    }
}
