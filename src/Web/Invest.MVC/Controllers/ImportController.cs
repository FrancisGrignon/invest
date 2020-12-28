using Invest.MVC.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Invest.MVC.Controllers
{
    public class ImportController : Controller
    {
        private readonly InvestContext _context;
        private readonly UnitOfWork _unitOfWork;

        public ImportController(InvestContext context)
        {
            _context = context;
            _unitOfWork = new UnitOfWork(_context);
        }

        public IActionResult Stock(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            id = id.ToUpper();

            var stock = _unitOfWork.StockRepository.GetBySymbol(id);

            if (null == stock)
            {
                stock = new Stock
                {
                    Symbol = id
                };

                switch (id)
                {
                    case "ABNB":
                        stock.Name = "Airbnb";
                        stock.Currency = Invest.MVC.Forex.USD;
                        break;
                    case "COST":
                        stock.Name = "Costco";
                        stock.Currency = Invest.MVC.Forex.USD;
                        break;
                    case "EMP.A":
                        stock.Name = "Empire";
                        stock.Currency = Invest.MVC.Forex.CAD;
                        break;
                    case "GOOGL":
                        stock.Name = "Alphabet";
                        stock.Currency = Invest.MVC.Forex.USD;
                        break;
                    case "L":
                        stock.Name = "Loblaw";
                        stock.Currency = Invest.MVC.Forex.CAD;
                        break;
                    case "MSFT":
                        stock.Name = "Microsoft";
                        stock.Currency = Invest.MVC.Forex.USD;
                        break;
                    case "NSDOY":
                        stock.Name = "Nintendo";
                        stock.Currency = Invest.MVC.Forex.USD;
                        break;
                    case "SHOP":
                        stock.Name = "Shopify";
                        stock.Currency = Invest.MVC.Forex.CAD;
                        break;
                    case "TSLA":
                        stock.Name = "Tesla";
                        stock.Currency = Invest.MVC.Forex.USD;
                        break;
                    default:
                        return NotFound();
                }

                _unitOfWork.StockRepository.Add(stock);
                _unitOfWork.SaveChanges();
            }

            string path = $"data/{id}.csv";
            string[] data;
            DateTime date;
            decimal value;

            // Read file using StreamReader. Reads file line by line    
            using (StreamReader file = new StreamReader(path))
            {
                int counter = 0;
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    if (0 == counter)
                    {
                        // Ignore
                    }
                    else
                    {
                        var ci = new CultureInfo("en-US");

                        data = ln.Split(';');
                        date = Convert.ToDateTime(data[0]);
                        value = Convert.ToDecimal(data[1], ci);

                        stock.Value = value;

                        _unitOfWork.StockRepository.TakeSnapshot(stock, date);
                    }

                    Console.WriteLine(ln);
                    counter++;
                }

                file.Close();
            }

            _unitOfWork.SaveChanges();

            return View("Index");
        }


        public IActionResult Forex(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            id = id.ToUpper();

            var forex = _unitOfWork.ForexRepository.GetByCurrency(id);

            if (null == forex)
            {
                forex = new Forex
                {
                    Currency = id,
                    ExchangeRate = 1
                };

                _unitOfWork.ForexRepository.Add(forex);
                _unitOfWork.SaveChanges();
            }
            else
            {
                forex.Currency = id;

                _unitOfWork.ForexRepository.Update(forex);
                _unitOfWork.SaveChanges();
            }

            string path = $"data/{id}.csv";
            string[] data;
            DateTime date;
            decimal exchangeRate;

            // Read file using StreamReader. Reads file line by line    
            using (StreamReader file = new StreamReader(path))
            {
                int counter = 0;
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    if (0 == counter)
                    {
                        // Ignore
                    }
                    else
                    {
                        var ci = new CultureInfo("en-US");

                        data = ln.Split(';');
                        date = Convert.ToDateTime(data[0]);
                        exchangeRate = Convert.ToDecimal(data[1], ci);

                        forex.ExchangeRate = exchangeRate;

                        _unitOfWork.ForexRepository.TakeSnapshot(forex, date.ToUniversalTime());
                    }

                    Console.WriteLine(ln);
                    counter++;
                }

                file.Close();
            }

            _unitOfWork.SaveChanges();

            return View("Index");
        }
    }
}
