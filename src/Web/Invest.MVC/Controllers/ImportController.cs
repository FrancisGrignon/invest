using Invest.MVC.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invest.MVC.Controllers
{
    [Authorize]
    public class ImportController : Controller
    {
        private readonly StockService _stockService;
        private readonly ImportService _importService;

        public ImportController(InvestContext context)
        {
            _stockService = new StockService();
            _importService = new ImportService(context);
        }

        public async Task<IActionResult> IndexAsync()
        {
            //var files = new Dictionary<string, string>
            //{
            //    { "COST", "Data/COST.csv" },
            //    { "GOOGL", "Data/GOOGL.csv" },
            //    { "MSFT", "Data/MSFT.csv" },
            //    { "NTDOY", "Data/NTDOY.csv" },
            //    { "SHOP.TO", "Data/SHOP.csv" },
            //    { "TSLA", "Data/TSLA.csv" },
            //    { "VFV.TO", "Data/VFV.csv" }
            //};

            //foreach (var (symbol, filePath) in files)
            //{
            //    await _stockService.UpdateStockValuesAsync(symbol, filePath);
            //}

            //System.IO.File.Delete("invest.db");
            //System.IO.File.Delete("invest.db-shm");
            //System.IO.File.Delete("invest.db-wal");

            _importService.Execute();

            return View("Index");
        }
    }
}
