using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeClient.Models
{
    public class StockRepository : IStockRepository
    {
        private ApplicationDbContext _context;
        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Stock GetStock(int id)
        {
            return _context.Stocks.Single(x => x.Id == id);
        }

        public IEnumerable<Stock> GetUserStocks(string username)
        {
            return _context.Stocks.Where(x => x.UserName == username);
        }

        public void AddStock(Stock stock)
        {
            _context.Stocks.Add(stock);
        }

        public void RemoveStock(Stock stock)
        {
            _context.Stocks.Remove(stock);
        }

        public bool TickerExistsForUser(string username, string ticker)
        {
            return _context.Stocks.Any(s => s.Code == ticker && s.UserName == username);
        }
    }
}
