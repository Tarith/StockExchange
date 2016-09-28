using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using StockExchangeClient.Models;

namespace StockExchangeTest
{
    [TestClass]
    public class StockServiceTest
    {
        [TestMethod]
        public void WebService_GetPriceReturntsInteger()
        {
            var stockService = new StockService.ServiceSoapClient();
            var result = stockService.GetStockPrice("ticker", "unittestuser@test.test");
            Assert.IsInstanceOfType(result, typeof(int));
        }

        [TestMethod]
        public void WebService_GetPriceWithUnregisteredUserReturntsInteger()
        {
            var stockService = new StockService.ServiceSoapClient();
            try
            {
                stockService.GetStockPrice("ticker", "wronguser");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(System.ServiceModel.FaultException));
                Assert.IsTrue(e.Message.Contains("System.UnauthorizedAccessException"));

                
            }
        }
    }
}
