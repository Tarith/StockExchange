using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeClient.Models
{
    public interface IUnitOfWork : IDisposable
    {
        IStockRepository Stocks {get;}
        void Save();
    }
}
