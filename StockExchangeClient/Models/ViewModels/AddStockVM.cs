using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeClient.Models
{
    public class AddStockVM
    {
        [Display(Name = "Stock Ticker")]
        [MaxLength(5, ErrorMessage = "Max length is 5 characters")]
        [Required]
        public string Code { get; set; }
    }
}
