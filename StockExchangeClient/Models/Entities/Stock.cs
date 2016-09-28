using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeClient.Models
{
    public class Stock
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(5, ErrorMessage = "Max length is 5 characters")]
        [Required]
        public string Code { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
