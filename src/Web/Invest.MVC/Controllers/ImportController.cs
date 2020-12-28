using Invest.MVC.Infrastructure;
using Invest.MVC.Infrastructure.Services;
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

        public IActionResult Investors()
        {
            var names = new string[] { "Aglaé", "Pénélope", "Étienne", "Cédric", "Anabelle", "Aaricia", "Marco", "Geneviève" };

            foreach (var name in names)
            {
                var investor = _unitOfWork.InvestorRepository.GetByName(name);

                if (null == investor)
                {
                    investor = new Investor
                    {
                        Name = name
                    };

                    _unitOfWork.InvestorRepository.Add(investor);
                }
            }

            _unitOfWork.SaveChanges();

            return View("Index");
        }

        public IActionResult Transactions()
        {
            var broker = new BrokerService(_unitOfWork);
            var investor = _unitOfWork.InvestorRepository.GetByName("Pénélope");

            var date = new DateTime(2019, 06, 07);

            broker.Deposit(investor, 100, Forex.CAD, date);

            var amount = broker.Transfer(investor, 100, Forex.CAD, Forex.USD, date);

            var stock = _unitOfWork.StockRepository.GetBySymbol("COST");
            var value = _unitOfWork.StockRepository.GetValue(stock, date);
            var quantity = amount / value;

            var investment = broker.Buy(investor, stock, quantity, date);

            _unitOfWork.SaveChanges();

            // Snapshot
            decimal exchangeRate;
            DateTime max = new DateTime(2020, 12, 25);

            while (date <= max)
            {
                value = _unitOfWork.StockRepository.GetValue(stock, date);
                exchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(Forex.CAD, Forex.USD, date);

                _unitOfWork.InvestmentRepository.TakeSnapshot(investment, date, value, exchangeRate);

                date = date.AddDays(7);
            }

            _unitOfWork.SaveChanges();

            return View("Index");
        }

        public IActionResult Stocks(string id)
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
                        stock.Currency = Forex.USD;
                        break;
                    case "COST":
                        stock.Name = "Costco";
                        stock.Currency = Forex.USD;
                        break;
                    case "EMP.A":
                        stock.Name = "Empire";
                        stock.Currency = Forex.CAD;
                        break;
                    case "GOOGL":
                        stock.Name = "Alphabet";
                        stock.Currency = Forex.USD;
                        break;
                    case "L":
                        stock.Name = "Loblaw";
                        stock.Currency = Forex.CAD;
                        break;
                    case "MSFT":
                        stock.Name = "Microsoft";
                        stock.Currency = Forex.USD;
                        break;
                    case "NSDOY":
                        stock.Name = "Nintendo";
                        stock.Currency = Forex.USD;
                        break;
                    case "SHOP":
                        stock.Name = "Shopify";
                        stock.Currency = Forex.CAD;
                        break;
                    case "TSLA":
                        stock.Name = "Tesla";
                        stock.Currency = Forex.USD;
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

        public IActionResult Forexes(string id)
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
