using StockExchangeClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeTest
{
    public class MockService : IStockService
    {
        Random rnd = new Random();
        public int GetStockPrice(string stock)
        {
            return rnd.Next(1, 1001);
        }
    }

    public class MockDataBase : IUnitOfWork
    {
        private List<Stock> mockDB;

        public MockDataBase() { }
        public MockDataBase(List<Stock> stockList)
        {
            mockDB = stockList;
        }

        public IStockRepository Stocks
        {
            get
            {
                if (mockDB != null)
                {
                    return new MockRepository(mockDB);
                }

                return new MockRepository();
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void Save()
        {
            //throw new NotImplementedException();
        }
    }

    public class MockRepository : IStockRepository
    {
        private List<Stock> mockDB = new List<Stock>();

        public MockRepository() { }
        public MockRepository(List<Stock> stocks)
        {
            mockDB = stocks;
        }

        public void AddStock(Stock stock)
        {
            mockDB.Add(stock);
        }

        public Stock GetStock(int id)
        {
           return mockDB.Find(x => x.Id == id);
        }

        public IEnumerable<Stock> GetUserStocks(string username)
        {
            return mockDB.FindAll(x => x.UserName == username);
        }

        public void RemoveStock(Stock stock)
        {
            mockDB.Remove(stock);
        }

        public bool TickerExistsForUser(string username, string ticker)
        {
            return mockDB.Any(x => x.UserName == username && x.Code == ticker);
        }
    }
}
