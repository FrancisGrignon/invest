using System;
using System.Globalization;
using System.IO;

namespace Invest.MVC.Infrastructure.Services
{
    public class ImportService
    {
        private UnitOfWork _unitOfWork;
        private DateTime _until { get; }

        public ImportService(InvestContext context) : this(new UnitOfWork(context)) 
        {

        }

        public ImportService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _until = FindLastFriday();
        }

        private DateTime FindLastFriday()
        {
            var now = DateTime.Now.ToUniversalTime().Date;

            int backInTime;

            if (DayOfWeek.Friday == now.DayOfWeek)
            {
                // all good
                backInTime = 0;
            }
            else if (DayOfWeek.Friday < now.DayOfWeek)
            {
                backInTime = 1;
            }
            else
            {
                backInTime = Convert.ToInt32(now.DayOfWeek) + 2;
            }

            var friday = now.AddDays(-backInTime);

            return friday;
        }

        public void Execute()
        {
            ImportForex(Forex.CAD);
            ImportForex(Forex.USD);

            ImportStock("ABNB");
            ImportStock("COST");
            ImportStock("EMP.A");
            ImportStock("GOOGL");
            ImportStock("L");
            ImportStock("MSFT");
            ImportStock("NTDOY");
            ImportStock("SHOP");
            ImportStock("TSLA");

            ImportInvestors();

            ImportOlderTransactions("Aglaé", "GOOGL");
            ImportOlderTransactions("Pénélope", "COST");
            ImportOlderTransactions("Geneviève", "SHOP");
            ImportEtienneTransactions();
            ImportOtherTransactions("Aaricia", "ABNB");
            ImportOtherTransactions("Annabelle", "L");
            ImportOtherTransactions("Cédrik", "TSLA");
            ImportOtherTransactions("Marco", "NTDOY");
        }

        public void ImportInvestors()
        {
            var names = new string[] { "Aglaé", "Pénélope", "Étienne", "Cédrik", "Annabelle", "Aaricia", "Marco", "Geneviève" };

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

            _unitOfWork.SaveChanges();

            // CAD to USD
            if (Forex.USD == stock.Currency)
            {
                amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);
            }

            _unitOfWork.SaveChanges();

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

            _unitOfWork.SaveChanges();

            // 100 Deposit
            broker.Deposit(investor, amount, Forex.CAD, date);

            _unitOfWork.SaveChanges();

            // CAD to USD
            if (Forex.USD == stock.Currency)
            {
                amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);
            }

            _unitOfWork.SaveChanges();

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            max = _until;

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

            _unitOfWork.SaveChanges();

            // CAD to USD
            var amount = broker.Transfer(investor, 100, Forex.CAD, Forex.USD, date);

            _unitOfWork.SaveChanges();

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

            _unitOfWork.SaveChanges();

            // 100 Deposit
            broker.Deposit(investor, 100, Forex.CAD, date);

            _unitOfWork.SaveChanges();

            // CAD to USD
            amount = broker.Transfer(investor, 100, Forex.CAD, Forex.USD, date);

            _unitOfWork.SaveChanges();

            // Buy
            stock = _unitOfWork.StockRepository.GetBySymbol("COST");
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            _unitOfWork.SaveChanges();

            // Take snapshot
            max = _until;

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

            _unitOfWork.SaveChanges();

            var amount = broker.Transfer(investor, 100, Forex.CAD, Forex.USD, date);

            _unitOfWork.SaveChanges();

            var stock = _unitOfWork.StockRepository.GetBySymbol("GOOGL");
            var value = _unitOfWork.StockRepository.GetValue(stock, date);
            var quantity = amount / value;

            var investment = broker.Buy(investor, stock, quantity, date);

            _unitOfWork.SaveChanges();

            // Snapshot
            decimal exchangeRate;
            DateTime max = new DateTime(2020, 06, 05);

            while (date <= max)
            {
                value = _unitOfWork.StockRepository.GetValue(stock, date);
                exchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(stock.Currency, Forex.CAD, date);

                _unitOfWork.InvestmentRepository.TakeSnapshot(investment, date, value, exchangeRate);

                date = date.AddDays(7);
            }

            _unitOfWork.SaveChanges();

            // 100 Deposit
            broker.Deposit(investor, 100, Forex.CAD, date);

            _unitOfWork.SaveChanges();

            // CAD to USD
            amount = broker.Transfer(investor, 100, Forex.CAD, Forex.USD, date);

            _unitOfWork.SaveChanges();

            // Buy
            stock = _unitOfWork.StockRepository.GetBySymbol("GOOGL");
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            _unitOfWork.SaveChanges();

            // Take snapshot
            max = _until;

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

            _unitOfWork.SaveChanges();

            var stock = _unitOfWork.StockRepository.GetBySymbol("SHOP");
            var value = _unitOfWork.StockRepository.GetValue(stock, date);
            var quantity = amount / value;

            var investment = broker.Buy(investor, stock, quantity, date);

            _unitOfWork.SaveChanges();

            // Snapshot
            decimal exchangeRate;
            DateTime max = new DateTime(2020, 06, 05);

            while (date <= max)
            {
                value = _unitOfWork.StockRepository.GetValue(stock, date);
                exchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(stock.Currency, Forex.CAD, date);

                _unitOfWork.InvestmentRepository.TakeSnapshot(investment, date, value, exchangeRate);

                date = date.AddDays(7);
            }

            _unitOfWork.SaveChanges();

            // 100 Deposit
            broker.Deposit(investor, 100, Forex.CAD, date);

            _unitOfWork.SaveChanges();

            // Buy
            stock = _unitOfWork.StockRepository.GetBySymbol("SHOP");
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            _unitOfWork.SaveChanges();

            // Take snapshot
            max = _until;

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

            _unitOfWork.SaveChanges();

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

            _unitOfWork.SaveChanges();

            // Sell IGA
            amount = broker.Sell(investor, stock, quantity, date);

            _unitOfWork.SaveChanges();

            // Deposit 100$
            broker.Deposit(investor, 100M, Forex.CAD, date);

            _unitOfWork.SaveChanges();

            amount = amount + 100M;

            // Transfer CAD to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            _unitOfWork.SaveChanges();

            // Buy Microsoft
            stock = _unitOfWork.StockRepository.GetBySymbol("MSFT");
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;

            investment = broker.Buy(investor, stock, quantity, date);

            _unitOfWork.SaveChanges();

            max = _until;

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

            _unitOfWork.SaveChanges();

            var stock = _unitOfWork.StockRepository.GetBySymbol(symbol);

            if (Forex.USD == stock.Currency)
            {
                amount = broker.Transfer(investor, 100, Forex.CAD, Forex.USD, date);

                _unitOfWork.SaveChanges();
            }

            var value = _unitOfWork.StockRepository.GetValue(stock, date);
            var quantity = amount / value;

            var investment = broker.Buy(investor, stock, quantity, date);

            _unitOfWork.SaveChanges();

            // Snapshot
            decimal exchangeRate;

            var max = _until;

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

        public void ImportStock(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
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
                        return;
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
        }

        public void ImportForex(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
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
        }
    }
}
