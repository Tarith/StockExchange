using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeClient.Models
{
    public interface IStockService
    {
        int GetStockPrice(string stock);
    }
}
