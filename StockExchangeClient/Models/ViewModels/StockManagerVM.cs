using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeClient.Models
{
    public class StockManagerVM
    {
        public int StockId { get; set; }
        public string Stock { get; set; }
        public int Price { get; set; }
    }
}
