using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeClient.Models
{
    public interface IStockRepository
    {
        Stock GetStock(int id);
        bool TickerExistsForUser(string username, string ticker);
        IEnumerable<Stock> GetUserStocks(string username);
        void AddStock(Stock stock);
        void RemoveStock(Stock stock);
    }
}
