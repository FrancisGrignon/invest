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
            ImportOlderTransactions("Aglaé", "GOOGL");
            ImportOlderTransactions("Pénélope", "COST");
            ImportOlderTransactions("Geneviève", "SHOP");

            //ImportPenelopeTransactions();
            //ImportAglaeTransactions();
            //ImportGenevieveTransactions();s
            ImportEtienneTransactions();
            
            ImportOtherTransactions("Aaricia", "ABNB");
            ImportOtherTransactions("Anabelle", "L");
            ImportOtherTransactions("Cédric", "TSLA");
            ImportOtherTransactions("Marco", "NTDOY");

            return View("Index");
        }

        public void ImportOlderTransactions(string investorName, string stockName)
        {
            var broker = new BrokerService(_unitOfWork);
            var investor = _unitOfWork.InvestorRepository.GetByName(investorName);
            var stock = _unitOfWork.StockRepository.GetBySymbol(stockName);
            decimal amount = 100M;

            var date = new DateTime(2019, 06, 07);

            // 100 Deposit
            broker.Deposit(investor, amount, Forex.CAD, date);

            // CAD to USD
            if (Forex.USD == stock.Currency)
            {
                amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);
            }            

            // Buy            
            var value = _unitOfWork.StockRepository.GetValue(stock, date);
            var quantity = amount / value;
            var investment = broker.Buy(investor, stock, quantity, date);

            _unitOfWork.SaveChanges();

            // Snapshot
            decimal exchangeRate;
            DateTime max = new DateTime(2020, 06, 05);

            // Take snapshot
            while (date <= max)
            {
                value = _unitOfWork.StockRepository.GetValue(stock, date);
                exchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(stock.Currency, Forex.CAD, date);

                _unitOfWork.InvestmentRepository.TakeSnapshot(investment, date, value, exchangeRate);

                date = date.AddDays(7);
            }

            amount = 100M;

            // 100 Deposit
            broker.Deposit(investor, amount, Forex.CAD, date);

            // CAD to USD
            if (Forex.USD == stock.Currency)
            {
                amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);
            }

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            max = new DateTime(2020, 12, 25);

            while (date <= max)
            {
                value = _unitOfWork.StockRepository.GetValue(stock, date);
                exchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(stock.Currency, Forex.CAD, date);

                _unitOfWork.InvestmentRepository.TakeSnapshot(investment, date, value, exchangeRate);

                date = date.AddDays(7);
            }

            _unitOfWork.SaveChanges();
        }

        public void ImportPenelopeTransactions()
        {
            var broker = new BrokerService(_unitOfWork);
            var investor = _unitOfWork.InvestorRepository.GetByName("Pénélope");

            var date = new DateTime(2019, 06, 07);

            // 100 Deposit
            broker.Deposit(investor, 100, Forex.CAD, date);

            // CAD to USD
            var amount = broker.Transfer(investor, 100, Forex.CAD, Forex.USD, date);

            // Buy
            var stock = _unitOfWork.StockRepository.GetBySymbol("COST");
            var value = _unitOfWork.StockRepository.GetValue(stock, date);
            var quantity = amount / value;
            var investment = broker.Buy(investor, stock, quantity, date);

            _unitOfWork.SaveChanges();

            // Snapshot
            decimal exchangeRate;
            DateTime max = new DateTime(2020, 06, 05);

            // Take snapshot
            while (date <= max)
            {
                value = _unitOfWork.StockRepository.GetValue(stock, date);
                exchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(stock.Currency, Forex.CAD, date);

                _unitOfWork.InvestmentRepository.TakeSnapshot(investment, date, value, exchangeRate);

                date = date.AddDays(7);
            }

            // 100 Deposit
            broker.Deposit(investor, 100, Forex.CAD, date);

            // CAD to USD
            amount = broker.Transfer(investor, 100, Forex.CAD, Forex.USD, date);

            // Buy
            stock = _unitOfWork.StockRepository.GetBySymbol("COST");
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            max = new DateTime(2020, 12, 25);

            while (date <= max)
            {
                value = _unitOfWork.StockRepository.GetValue(stock, date);
                exchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(stock.Currency, Forex.CAD, date);

                _unitOfWork.InvestmentRepository.TakeSnapshot(investment, date, value, exchangeRate);

                date = date.AddDays(7);
            }

            _unitOfWork.SaveChanges();
        }

        public void ImportAglaeTransactions()
        {
            var broker = new BrokerService(_unitOfWork);
            var investor = _unitOfWork.InvestorRepository.GetByName("Aglaé");

            var date = new DateTime(2019, 06, 07);

            broker.Deposit(investor, 100, Forex.CAD, date);

            var amount = broker.Transfer(investor, 100, Forex.CAD, Forex.USD, date);

            var stock = _unitOfWork.StockRepository.GetBySymbol("GOOGL");
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
                exchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(stock.Currency, Forex.CAD, date);

                _unitOfWork.InvestmentRepository.TakeSnapshot(investment, date, value, exchangeRate);

                date = date.AddDays(7);
            }

            _unitOfWork.SaveChanges();
        }

        public void ImportGenevieveTransactions()
        {
            var broker = new BrokerService(_unitOfWork);
            var investor = _unitOfWork.InvestorRepository.GetByName("Geneviève");

            var date = new DateTime(2019, 06, 07);
            var amount = 100M;

            broker.Deposit(investor, amount, Forex.CAD, date);

            var stock = _unitOfWork.StockRepository.GetBySymbol("SHOP");
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
                exchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(stock.Currency, Forex.CAD, date);

                _unitOfWork.InvestmentRepository.TakeSnapshot(investment, date, value, exchangeRate);

                date = date.AddDays(7);
            }

            _unitOfWork.SaveChanges();
        }


        public void ImportEtienneTransactions()
        {
            var broker = new BrokerService(_unitOfWork);
            var investor = _unitOfWork.InvestorRepository.GetByName("Étienne");

            // Deposit 100 CAD
            var date = new DateTime(2019, 06, 07);
            var amount = 100M;

            broker.Deposit(investor, amount, Forex.CAD, date);

            // Buy IGA
            var stock = _unitOfWork.StockRepository.GetBySymbol("EMP.A");
            var value = _unitOfWork.StockRepository.GetValue(stock, date);
            var quantity = amount / value;

            var investment = broker.Buy(investor, stock, quantity, date);

            _unitOfWork.SaveChanges();

            // Take snapshots
            decimal exchangeRate;
            DateTime max = new DateTime(2020, 06, 05);

            while (date <= max)
            {
                value = _unitOfWork.StockRepository.GetValue(stock, date);
                exchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(stock.Currency, Forex.CAD, date);

                _unitOfWork.InvestmentRepository.TakeSnapshot(investment, date, value, exchangeRate);

                date = date.AddDays(7);
            }

            // Sell IGA
            amount = broker.Sell(investor, stock, quantity, date);
            
            // Deposit 100$
            broker.Deposit(investor, 100M, Forex.CAD, date);

            amount = amount + 100M;

            // Transfer CAD to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy Microsoft
            stock = _unitOfWork.StockRepository.GetBySymbol("MSFT");
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;

            investment = broker.Buy(investor, stock, quantity, date);

            max = new DateTime(2020, 12, 25);

            // Take snapshots
            while (date <= max)
            {
                value = _unitOfWork.StockRepository.GetValue(stock, date);
                exchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(stock.Currency, Forex.CAD, date);

                _unitOfWork.InvestmentRepository.TakeSnapshot(investment, date, value, exchangeRate);

                date = date.AddDays(7);
            }

            _unitOfWork.SaveChanges();
        }

        public void ImportOtherTransactions(string investorName, string symbol)
        {
            var broker = new BrokerService(_unitOfWork);
            var investor = _unitOfWork.InvestorRepository.GetByName(investorName);

            var date = new DateTime(2020, 12, 25);

            decimal amount = 100M;

            broker.Deposit(investor, amount, Forex.CAD, date);

            var stock = _unitOfWork.StockRepository.GetBySymbol(symbol);

            if (Forex.USD == stock.Currency)
            {
                amount = broker.Transfer(investor, 100, Forex.CAD, Forex.USD, date);
            }

            var value = _unitOfWork.StockRepository.GetValue(stock, date);
            var quantity = amount / value;

            var investment = broker.Buy(investor, stock, quantity, date);

            _unitOfWork.SaveChanges();

            // Snapshot
            decimal exchangeRate;

            value = _unitOfWork.StockRepository.GetValue(stock, date);
            exchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(stock.Currency, Forex.CAD, date);

            _unitOfWork.InvestmentRepository.TakeSnapshot(investment, date, value, exchangeRate);

            _unitOfWork.SaveChanges();
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
                    case "NTDOY":
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
