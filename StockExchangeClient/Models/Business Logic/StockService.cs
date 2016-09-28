using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StockExchangeClient.Models
{
    public class StockService : IStockService
    {
        public int GetStockPrice(string stock)
        {
            using (var serviceConsumer = new StockExchangeService.ServiceSoapClient())
            {
                return serviceConsumer.GetStockPrice(stock, HttpContext.Current.User.Identity.Name);
            }
        }
    }
}
