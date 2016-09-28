using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeClient.Models
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        private IStockRepository _stockRepository;
        private bool _disposed = false;

        public IStockRepository Stocks
        {
            get
            {
                if (_stockRepository == null)
                {
                    _stockRepository = new StockRepository(_context);
                }

                return _stockRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
