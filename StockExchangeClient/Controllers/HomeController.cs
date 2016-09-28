using StockExchangeClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StockExchangeClient.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IStockService _service;

        public HomeController(IUnitOfWork db, IStockService service)
        {
            _db = db;
            _service = service;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult GetTicker()
        {
            var stockManagerList = new List<StockManagerVM>();

            var myStocks = _db.Stocks.GetUserStocks(User.Identity.Name);

            foreach (var item in myStocks)
            {
                var stockItem = new StockManagerVM()
                {
                    StockId = item.Id,
                    Stock = item.Code,
                    Price = GetStockValue(item.Code)
                };

                stockManagerList.Add(stockItem);
            }

            return PartialView("_Ticker", stockManagerList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStock(AddStockVM stockItem)
        {
            if (ModelState.IsValid)
            {

                if (!_db.Stocks.TickerExistsForUser(User.Identity.Name, stockItem.Code))
                {
                    var stock = new Stock();
                    stock.UserName = User.Identity.Name;
                    stock.Code = stockItem.Code.ToUpper();

                    _db.Stocks.AddStock(stock);

                    _db.Save();

                    return Json(new { success = true, message = "" });
                }
                else
                {
                    return Json(new { success = false, message = "The code already exists for this user" });
                }
            }

            return Json(new { success = false, message = "" });

        }
        
        public ActionResult DeleteStock(int stockToDeleteId)
        {
            var stock = _db.Stocks.GetStock(stockToDeleteId);
            _db.Stocks.RemoveStock(stock);
            _db.Save();

            return Json(new { success = true, message = "" });
           
        }
        
        private int GetStockValue(string stock)
        {
            return _service.GetStockPrice(stock);
        }        
    }
}