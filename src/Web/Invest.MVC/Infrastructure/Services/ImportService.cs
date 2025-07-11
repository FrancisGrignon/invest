﻿using System;
using System.Globalization;
using System.IO;

namespace Invest.MVC.Infrastructure.Services
{
    public class ImportService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly DateTime _until;
        private readonly CultureInfo _ci = new CultureInfo("en-US");

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

            ImportStock("ABNB", "Airbnb", "NASDAQ", Forex.USD);
            ImportStock("COST", "Costco", "NASDAQ", Forex.USD);
            ImportStock("EMP.A", "Empire", "TSE", Forex.CAD);
            ImportStock("GOOGL", "Alphabet", "NASDAQ", Forex.USD);
            ImportStock("L", "Loblaw", "TSE", Forex.CAD);
            ImportStock("MSFT", "Microsoft", "NASDAQ", Forex.USD);
            ImportStock("NTDOY", "Nintendo", "OTCMKTS", Forex.USD);
            ImportStock("SHOP", "Shopify", "TSE", Forex.CAD);
            ImportStock("TSLA", "Tesla", "NASDAQ", Forex.USD);
            ImportStock("VFV", "S&P 500", "TSE", Forex.CAD);
            ImportStock("BAM.A", "Brookfield", "TSE", Forex.CAD);
            ImportStock("BAM", "Brookfield", "TSE", Forex.CAD);
            ImportStock("BN", "Brookfield Corporation", "TSE", Forex.CAD);

            ImportInvestors();

            ImportAglaeTransactions();
            ImportPenelopeTransactions();
            ImportGenevieveTransactions();
            ImportEtienneTransactions();
            ImportAariciaTransactions();
            ImportCedrikTransactions();
            ImportMarcoTransactions();

            ImportAnabelleTransactions();
        }

        //public void ImportJournal()
        //{
        //    string path = $"data/journal.csv";
        //    string[] data;
        //    DateTime date;
        //    float quantity, amount = 0M, value;
        //    string action, name, extra, symbol, forex = Forex.CAD;
        //    Investor investor;
        //    Stock stock;
        //    Investment investment;

        //    var broker = new BrokerService(_unitOfWork);

        //    // Read file using StreamReader. Reads file line by line    
        //    using (StreamReader file = new StreamReader(path))
        //    {
        //        string ln;

        //        while ((ln = file.ReadLine()) != null)
        //        {
        //            data = ln.Split(';');
        //            date = Convert.ToDateTime(data[0]);
        //            name = data[1];
        //            action = data[2];
        //            extra = data[3];

        //            investor = _unitOfWork.InvestorRepository.GetByName(name);

        //            if ("deposit" == action)
        //            {
        //                amount = Convert.ToDecimal(extra, _ci);
        //                broker.Deposit(investor, amount, Forex.CAD, date);

        //                _unitOfWork.SaveChanges();
        //            }
        //            else if ("buy" == action)
        //            {
        //                // Get investor balance

        //                // Convert balance if required

        //                // Buy stock

        //                // Takes snapshots

        //                symbol = extra;
        //                stock = _unitOfWork.StockRepository.GetBySymbol(symbol);

        //                // CAD to USD
        //                if (forex != stock.Currency)
        //                {
        //                    amount = broker.Transfer(investor, amount, forex, stock.Currency, date);

        //                    _unitOfWork.SaveChanges();
        //                }

        //                // Buy
        //                value = _unitOfWork.StockRepository.GetValue(stock, date);
        //                quantity = amount / value;
        //                investment = broker.Buy(investor, stock, quantity, date);

        //                amount = 0M;
        //            }
        //            else if ("sell" == action)
        //            {
        //                symbol = extra;
        //                stock = _unitOfWork.StockRepository.GetBySymbol(symbol);
        //                investment = _unitOfWork.InvestmentRepository.GetByInvestor(investor, stock);

        //                broker.Sell(investor, stock, investment.Quantity, date);

        //                _unitOfWork.SaveChanges();
        //            }
        //        }
        //    }
        //}

        public void ImportInvestors()
        {
            Console.WriteLine($"ImportInvestors()");

            var names = new string[] { "Aglaé", "Pénélope", "Étienne", "Cédrik", "Anabelle", "Aaricia", "Marco", "Geneviève" };

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

        public void ImportPenelopeTransactions()
        {
            var investorName = "Pénélope";
            var stockName = "COST";

            Console.WriteLine($"ImportOlderTransactions({investorName},{stockName})");

            var broker = new BrokerService(_unitOfWork);
            var investor = _unitOfWork.InvestorRepository.GetByName(investorName);
            var stock = _unitOfWork.StockRepository.GetBySymbol(stockName);
            var amount = 0f;

            DateTime date = new DateTime(2019, 06, 07);

            // 2019

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            if (Forex.USD == stock.Currency)
            {
                amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);
            }

            // Buy            
            var value = _unitOfWork.StockRepository.GetValue(stock, date);
            var quantity = amount / value;
            var investment = broker.Buy(investor, stock, quantity, date);

            // Snapshot
            date = Snapshot(investment, date, new DateTime(2020, 06, 05));

            // 2020

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            if (Forex.USD == stock.Currency)
            {
                amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);
            }

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2021, 06, 4));

            // 2021
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            if (Forex.USD == stock.Currency)
            {
                amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);
            }

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot 2022
            date = Snapshot(investment, date, new DateTime(2022, 06, 10));

            // 2022
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            if (Forex.USD == stock.Currency)
            {
                amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);
            }

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot 2023
            date = Snapshot(investment, date, new DateTime(2023, 06, 23));

            // 2023
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            if (Forex.USD == stock.Currency)
            {
                amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);
            }

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2023, 12, 29));

            // Cadeau Noel Martine

            // 60 Deposit
            amount = broker.Deposit(investor, 60f, Forex.CAD, date);

            // Convert to USD
            if (Forex.USD == stock.Currency)
            {
                amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);
            }

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot 2024
            date = Snapshot(investment, date, new DateTime(2024, 06, 28));

            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            if (Forex.USD == stock.Currency)
            {
                amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);
            }

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot until Christmas 2024
            date = Snapshot(investment, date, new DateTime(2024, 12, 13));

            // Martine and Carl gift
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2025, 06, 27));

            // 2025

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            Snapshot(investment, date, _until);
        }
        public void ImportAglaeTransactions()
        {
            Console.WriteLine($"ImportAglaeTransactions()");

            var broker = new BrokerService(_unitOfWork);
            var investor = _unitOfWork.InvestorRepository.GetByName("Aglaé");
            var stock = _unitOfWork.StockRepository.GetBySymbol("GOOGL");

            DateTime date = new DateTime(2019, 06, 07);

            // 2019

            // 100 Deposit
            var amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy            
            var value = _unitOfWork.StockRepository.GetValue(stock, date);
            var quantity = amount / value;
            var investment = broker.Buy(investor, stock, quantity, date);

            // Snapshot
            date = Snapshot(investment, date, new DateTime(2020, 06, 05));

            // 2020

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2021, 06, 4));

            // 2021

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot 2022
            date = Snapshot(investment, date, new DateTime(2022, 06, 10));

            // 2022

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2022, 07, 15));

            // Split - 2022-07-23
            investment = broker.Split(investor, stock, 20, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2023, 06, 23));

            // 2023

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2023, 12, 29));

            // Cadeau Noel Martine

            // 60 Deposit
            amount = broker.Deposit(investor, 60f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2024, 06, 28));

            // 2024

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot until Christmas 2024
            date = Snapshot(investment, date, new DateTime(2024, 12, 13));

            // Martine and Carl gift
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2025, 06, 27));

            // 2025

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshots
            date = Snapshot(investment, date, new DateTime(2025, 07, 04));

            // Sell
            amount = broker.Sell(investment, date);

            // Convert to CAD
            amount = broker.Transfer(investor, amount, Forex.USD, Forex.CAD, date);

            // Withdraw
            broker.Withdraw(investor, amount, Forex.CAD, date);

            Console.WriteLine($"Aglaé end result: {amount} CAD");

            Snapshot(investment, date, date);
        }

        public void ImportCedrikTransactions()
        {
            Console.WriteLine($"ImportCedrikTransactions()");

            var broker = new BrokerService(_unitOfWork);
            var investor = _unitOfWork.InvestorRepository.GetByName("Cédrik");
            var stock = _unitOfWork.StockRepository.GetBySymbol("TSLA");

            // 2020
            var date = new DateTime(2020, 12, 25);

            // 100 Deposit
            var amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy            
            var value = _unitOfWork.StockRepository.GetValue(stock, date);
            var quantity = amount / value;
            var investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2021, 12, 17));

            // 2021

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2022, 08, 19));

            // Split - 2022-08-26
            investment = broker.Split(investor, stock, 3, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2022, 12, 23));

            // 2022

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshots
            date = Snapshot(investment, date, new DateTime(2023, 12, 15));

            // 2023

            // Sell VFV
            amount = broker.Sell(investment, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.USD, Forex.CAD, date);

            // Withdraw
            broker.Withdraw(investor, amount, Forex.CAD, date);

            Console.WriteLine($"Cedrik end result: {amount} CAD");

            Snapshot(investment, date, date);
        }

        public void ImportEtienneTransactions()
        {
            Console.WriteLine($"ImportEtienneTransactions()");

            var broker = new BrokerService(_unitOfWork);
            var investor = _unitOfWork.InvestorRepository.GetByName("Étienne");

            // 2019
            var date = new DateTime(2019, 06, 07);
            var amount = 100f;

            broker.Deposit(investor, amount, Forex.CAD, date);

            // Buy IGA
            var stock = _unitOfWork.StockRepository.GetBySymbol("EMP.A");
            var value = _unitOfWork.StockRepository.GetValue(stock, date);
            var quantity = amount / value;

            var investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshots
            date = Snapshot(investment, date, new DateTime(2020, 06, 05));

            // 2020

            // Sell IGA
            amount = broker.Sell(investor, stock, quantity, date);

            // Deposit 100$
            amount += broker.Deposit(investor, 100f, Forex.CAD, date);

            // Transfer CAD to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy Microsoft
            stock = _unitOfWork.StockRepository.GetBySymbol("MSFT");
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;

            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshots
            date = Snapshot(investment, date, new DateTime(2021, 06, 04));

            // 2021

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshots
            date = Snapshot(investment, date, new DateTime(2022, 06, 10));

            // 2022

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshots
            date = Snapshot(investment, date, new DateTime(2023, 06, 23));

            // 2023

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2023, 12, 29));

            // Cadeau Noel Martine

            // 60 Deposit
            amount = broker.Deposit(investor, 60f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2024, 06, 28));

            // 2024

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot until Christmas 2024
            date = Snapshot(investment, date, new DateTime(2024, 12, 13));

            // Martine and Carl gift
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2025, 06, 27));

            // 2025

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            Snapshot(investment, date, _until);
        }

        public void ImportGenevieveTransactions()
        {
            var investorName = "Geneviève";
            var stockName = "SHOP";

            Console.WriteLine($"ImportGeneviveTransactions()");

            var broker = new BrokerService(_unitOfWork);
            var investor = _unitOfWork.InvestorRepository.GetByName(investorName);
            var stock = _unitOfWork.StockRepository.GetBySymbol(stockName);

            // 2019

            float amount = 100f;
            var date = new DateTime(2019, 06, 07);

            // 100 Deposit
            broker.Deposit(investor, amount, Forex.CAD, date);

            // Buy            
            var value = _unitOfWork.StockRepository.GetValue(stock, date);
            var quantity = amount / value;
            var investment = broker.Buy(investor, stock, quantity, date);

            // Snapshot
            date = Snapshot(investment, date, new DateTime(2020, 06, 05));

            // 2020

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2021, 06, 4));

            // 2021

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2022, 06, 10));

            // 2022

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            if (false) { 
                value = _unitOfWork.StockRepository.GetValue(stock, date);
                quantity = amount / value;

                investment = broker.Buy(investor, stock, quantity, date);

                // Take snapshot
                date = Snapshot(investment, date, new DateTime(2022, 06, 24));

                // Split - 2022-07-01
                investment = broker.Split(investor, stock, 10, date);
            }
            else { 
                // Sell SHOP
                amount += broker.Sell(investor, stock, investment.Quantity, date);

                // Buy BAM.A
                stock = _unitOfWork.StockRepository.GetBySymbol("BAM.A");
                value = _unitOfWork.StockRepository.GetValue(stock, date);
                quantity = amount / value;

                investment = broker.Buy(investor, stock, quantity, date);

                // Take snapshot
                date = Snapshot(investment, date, new DateTime(2022, 12, 02));

                // Split BAM.A into BAM and BN
                amount = broker.Sell(investor, stock, investment.Quantity, date);

                stock = _unitOfWork.StockRepository.GetBySymbol("BAM");
                value = _unitOfWork.StockRepository.GetValue(stock, date);
                quantity = amount / value;

                investment = broker.Buy(investor, stock, quantity, date);
            }

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2023, 06, 23));

            // 2023

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshots
            date = Snapshot(investment, date, new DateTime(2023, 12, 29));

            // 2023

            // Sell BAM
            amount = broker.Sell(investment, date);

            broker.Withdraw(investor, amount, Forex.CAD, date);

            Console.WriteLine($"Geneviève end result: {amount} CAD");

            // Take snapshot
            Snapshot(investment, date, date);
        }

        public void ImportMarcoTransactions()
        {
            Console.WriteLine($"ImportMarcoTransactions()");

            var broker = new BrokerService(_unitOfWork);
            var investor = _unitOfWork.InvestorRepository.GetByName("Marco");
            var stock = _unitOfWork.StockRepository.GetBySymbol("NTDOY");

            // 2020
            var date = new DateTime(2020, 12, 25);

            // 100 Deposit
            var amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy            
            var value = _unitOfWork.StockRepository.GetValue(stock, date);
            var quantity = amount / value;
            var investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2021, 12, 17));

            // 2021

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date); ;

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2022, 09, 30));

            // Split - 2022-10-07
            investment = broker.Split(investor, stock, 5, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2022, 12, 23));

            // 2022

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;

            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2023, 12, 15));

            // 2023

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date); ;

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;

            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2023, 12, 29));

            // Cadeau Noel Martine

            // 60 Deposit
            amount = broker.Deposit(investor, 60f, Forex.CAD, date);

            // Convert to USD
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot until Christmas 2024
            date = Snapshot(investment, date, new DateTime(2024, 12, 13));

            // Martine and Carl gift
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Francis' gift
            amount += broker.Deposit(investor, 100f, Forex.CAD, date);

            // Buy
            amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            Snapshot(investment, date, _until);
        }

        public void ImportAariciaTransactions()
        {
            Console.WriteLine($"ImportAariciaTransactions()");

            var investorName = "Aaricia";
            var symbol = "ABNB";

            var broker = new BrokerService(_unitOfWork);
            var investor = _unitOfWork.InvestorRepository.GetByName(investorName);

            var date = new DateTime(2020, 12, 25);

            float amount = 100f;

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
            float exchangeRate;

            var max = new DateTime(2021, 12, 17);

            // Take snapshots
            while (date <= max)
            {
                value = _unitOfWork.StockRepository.GetValue(stock, date);
                exchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(stock.Currency, Forex.CAD, date);

                _unitOfWork.InvestmentRepository.TakeSnapshot(investment, date, value, exchangeRate);

                date = date.AddDays(7);
            }

            _unitOfWork.SaveChanges();

            // 100$ more
            amount = 100f;

            broker.Deposit(investor, amount, Forex.CAD, date);

            if (Forex.USD == stock.Currency)
            {
                amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);
            }

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2022, 12, 23));

            // 2022

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            if (Forex.USD == stock.Currency)
            {
                amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);
            }

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;

            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2023, 12, 15));

            // 2023

            // 100 Deposit
            amount = broker.Deposit(investor, 100f, Forex.CAD, date);

            // Convert to USD
            if (Forex.USD == stock.Currency)
            {
                amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);
            }

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;

            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2023, 12, 29));

            // Cadeau Noel Martine

            // 60 Deposit
            amount = broker.Deposit(investor, 60f, Forex.CAD, date);

            // Convert to USD
            if (Forex.USD == stock.Currency)
            {
                amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);
            }

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;
            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshots
            date = Snapshot(investment, date, new DateTime(2024, 05, 24));

            // 2024

            // Sell ABNB
            amount = broker.Sell(investment, date);

            // Convert to CAD
            amount = broker.Transfer(investor, amount, Forex.USD, Forex.CAD, date);

            // Withdraw
            broker.Withdraw(investor, amount, Forex.CAD, date);

            Console.WriteLine($"Aaricia end result: {amount} CAD");

            Snapshot(investment, date, date);
        }

        public void ImportAnabelleTransactions()
        {
            Console.WriteLine($"ImportAnabelleTransactions()");

            var broker = new BrokerService(_unitOfWork);
            var investor = _unitOfWork.InvestorRepository.GetByName("Anabelle");

            var date = new DateTime(2020, 12, 25);

            // Deposit 100 CAD
            float amount = 100f;

            broker.Deposit(investor, amount, Forex.CAD, date);

            _unitOfWork.SaveChanges();

            // Buy Loblaw
            var stock = _unitOfWork.StockRepository.GetBySymbol("L");
            var value = _unitOfWork.StockRepository.GetValue(stock, date);
            var quantity = amount / value;

            var investment = broker.Buy(investor, stock, quantity, date);

            _unitOfWork.SaveChanges();

            // Take snapshots
            date = Snapshot(investment, date, new DateTime(2021, 12, 17));

            _unitOfWork.SaveChanges();

            // Sell Loblaw
            amount = broker.Sell(investor, stock, quantity, date);

            _unitOfWork.SaveChanges();

            // Deposit 100$
            amount += broker.Deposit(investor, 100f, Forex.CAD, date);

            _unitOfWork.SaveChanges();

            // Buy S&P 500
            stock = _unitOfWork.StockRepository.GetBySymbol("VFV");
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;

            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshot
            date = Snapshot(investment, date, new DateTime(2022, 12, 23));

            // 2022
            amount = broker.Deposit(investor, 100f, Forex.CAD, stock.Currency, date);

            // Buy
            value = _unitOfWork.StockRepository.GetValue(stock, date);
            quantity = amount / value;

            investment = broker.Buy(investor, stock, quantity, date);

            // Take snapshots
            date = Snapshot(investment, date, new DateTime(2023, 12, 15));

            // 2023

            // Sell VFV
            amount = broker.Sell(investment, date);
            
            broker.Withdraw(investor, amount, Forex.CAD, date);

            Console.WriteLine($"Anabelle end result: {amount} CAD");

            Snapshot(investment, date, date);
        }

        public void Buy(string investorName, DateTime date, string symbol, string amount)
        {

        }

        public float Sell(string investorName, DateTime date, string symbol)
        {
            var broker = new BrokerService(_unitOfWork);
            var investor = _unitOfWork.InvestorRepository.GetByName(investorName);
            var stock = _unitOfWork.StockRepository.GetBySymbol(symbol);
            var investment = _unitOfWork.InvestmentRepository.GetByStock(stock);

            var amount = broker.Sell(investor, stock, investment.Quantity, date);

            _unitOfWork.SaveChanges();

            return amount;
        }

        public void Deposit(string investorName, DateTime date, float amount)
        {
            var broker = new BrokerService(_unitOfWork);
            var investor = _unitOfWork.InvestorRepository.GetByName(investorName);

            broker.Deposit(investor, amount, Forex.CAD, date);

            _unitOfWork.SaveChanges();
        }

        public void Transfer(string investorName, DateTime date, float amount, string currency)
        {
            if (Forex.USD == currency)
            {
                var investor = _unitOfWork.InvestorRepository.GetByName(investorName);
                var broker = new BrokerService(_unitOfWork);

                amount = broker.Transfer(investor, amount, Forex.CAD, Forex.USD, date);

                _unitOfWork.SaveChanges();
            }
        }

        public void Withdraw(string investorName, DateTime date)
        {

        }

        public DateTime Snapshot(Investment investment, DateTime startDate, DateTime endDate)
        {
            var date = startDate;
            var value = 0f;
            var exchangeRate = 0f;

            // Take snapshots
            while (date <= endDate)
            {
                value = _unitOfWork.StockRepository.GetValue(investment.Stock, date);
                exchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(investment.Stock.Currency, Forex.CAD, date);

                _unitOfWork.InvestmentRepository.TakeSnapshot(investment, date, value, exchangeRate);

                date = date.AddDays(7);
            }

            _unitOfWork.SaveChanges();

            return date;
        }

        public void ImportStock(string id, string name, string market, string currency)
        {
            Console.WriteLine($"ImportStock({id}, {name}, {market}, {currency})");

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
                    Symbol = id,
                    Name = name,
                    Market = market,
                    Currency = currency,
                    Split = 1
                };

                _unitOfWork.StockRepository.Add(stock);
                _unitOfWork.SaveChanges();
            }

            string path = $"data/{id}.csv";
            string[] data;
            DateTime date;
            float value, split;

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
                        value = float.Parse(data[1], ci);

                        if (3 < data.Length)
                        {
                            split = float.Parse(data[3], ci);
                        }

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
            Console.WriteLine($"ImportForex({id})");

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
            float exchangeRate;

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
                        exchangeRate = float.Parse(data[1], ci);

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
