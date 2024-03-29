﻿using System;

namespace Invest.MVC.Infrastructure.Services
{
    public class BrokerService
    {
        private UnitOfWork _unitOfWork;

        public BrokerService(InvestContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }

        public BrokerService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Investment Buy(Investor investor, Stock stock, float quantity, DateTime? date = null)
        {
            var dateUtc = ConvertDateToUtc(date);

            // Check if enough money!
            var value = _unitOfWork.StockRepository.GetValue(stock, dateUtc);

            var amount = quantity * value;

            var transaction = new Transaction
            {
                Investor = investor,
                OperationId = Operation.Buy,
                Stock = stock,
                Quantity = quantity,
                Amount = amount,
                Currency = stock.Currency,
                DateUtc = dateUtc,
                ExchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(stock.Currency, Forex.CAD, dateUtc)
            };

            _unitOfWork.TransactionRepository.Add(transaction);

            // Add stocks
            var investment = _unitOfWork.InvestmentRepository.GetByStock(stock);

            if (null == investment)
            {
                investment = new Investment
                {
                    CreatedUtc = DateTime.UtcNow,
                    Currency = stock.Currency,
                    Enable = true,
                    Investor = investor,
                    Quantity = quantity,
                    Stock = stock
                };

                _unitOfWork.InvestmentRepository.Add(investment);
            }
            else
            {
                investment.Quantity = investment.Quantity + quantity;

                _unitOfWork.InvestmentRepository.Update(investment);
            }

            _unitOfWork.SaveChanges();

            return investment;
        }

        public float Sell(Investment investment, DateTime? date = null)
        {
            return Sell(investment.Investor, investment.Stock, investment.Quantity, date);
        }

        public float Sell(Investor investor, Stock stock, float quantity, DateTime? date = null)
        {
            var dateUtc = ConvertDateToUtc(date);

            var investment = _unitOfWork.InvestmentRepository.GetByStock(stock);

            if (null == investment)
            {
                throw new Exception($"The investor {investor.Name} doesn't own the stock {stock.Name}.");
            }

            if (investment.Quantity < quantity)
            {
                throw new Exception($"The investor {investor.Name} doesn't own that {quantity} stock of {stock.Name}.");
            }

            var value = _unitOfWork.StockRepository.GetValue(stock, dateUtc);

            // Check if how stock
            var amount = quantity * value;

            // Add money
            var transaction = new Transaction
            {
                Investor = investor,
                OperationId = Operation.Sell,
                Stock = stock,
                Quantity = quantity,
                Amount = amount,
                Currency = stock.Currency,
                DateUtc = dateUtc,
                ExchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(stock.Currency, Forex.CAD, dateUtc)
            };

            _unitOfWork.TransactionRepository.Add(transaction);

            // Reduce stocks
            investment.Quantity = investment.Quantity - quantity;

            _unitOfWork.InvestmentRepository.Update(investment);
            
            _unitOfWork.SaveChanges();

            return amount;
        }

        public Money Deposit(Investor investor, Money money, DateTime? date = null)
        {
            Deposit(investor, money.Amount, money.Currency, date);

            return money;
        }

        public float Deposit(Investor investor, float amount, string currency, DateTime? date = null)
        {
            return Deposit(investor, amount, currency, currency, date);
        }

        public float Deposit(Investor investor, float amount, string sourceCurrency, string destinationCurrency, DateTime? date = null)
        {
            var dateUtc = ConvertDateToUtc(date);

            var exchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(sourceCurrency, destinationCurrency, dateUtc);

            amount = amount * exchangeRate;

            var transaction = new Transaction
            {
                Investor = investor,
                OperationId = Operation.Deposit,
                Amount = amount,
                Currency = destinationCurrency,
                DateUtc = dateUtc,
                ExchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(destinationCurrency, Forex.CAD, dateUtc)
            };

            _unitOfWork.TransactionRepository.Add(transaction);
            _unitOfWork.SaveChanges();

            return amount;
        }

        public void Withdraw(Investor investor, float amount, string currency, DateTime? date = null)
        {
            var dateUtc = ConvertDateToUtc(date);

            var transaction = new Transaction
            {
                Investor = investor,
                OperationId = Operation.Withdraw,
                Amount = amount,
                Currency = currency,
                DateUtc = dateUtc,
                ExchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(currency, Forex.CAD, dateUtc)
            };

            _unitOfWork.TransactionRepository.Add(transaction);
            _unitOfWork.SaveChanges();
        }

        public void Dividend(Investor investor, Stock stock, float amount, DateTime? date = null)
        {
            var dateUtc = ConvertDateToUtc(date);

            var transaction = new Transaction
            {
                Investor = investor,
                OperationId = Operation.Dividend,
                Stock = stock,
                Amount = amount,
                Currency = stock.Currency,
                DateUtc = dateUtc,
                ExchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(stock.Currency, Forex.CAD, dateUtc)
            };

            _unitOfWork.TransactionRepository.Add(transaction);
            _unitOfWork.SaveChanges();
        }

        public Investment Split(Investor investor, Stock stock, int ratio, DateTime? date = null)
        {
            var dateUtc = ConvertDateToUtc(date);

            // Update the quantity
            var investment = _unitOfWork.InvestmentRepository.GetByStock(stock);

            investment.Quantity *= ratio;

            // Record the transaction
            var transaction = new Transaction
            {
                Investor = investor,
                OperationId = Operation.Split,
                Stock = stock,
                Amount = 0,
                Quantity = investment.Quantity,
                Currency = stock.Currency,
                DateUtc = dateUtc,
                ExchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(stock.Currency, Forex.CAD, dateUtc)
            };

            _unitOfWork.InvestmentRepository.Update(investment);
            _unitOfWork.TransactionRepository.Add(transaction);
            _unitOfWork.SaveChanges();

            return investment;
        }

        public void Merge(Investor investor, Stock stock, int ratio, DateTime? date = null)
        {

        }

        public float Transfer(Investor investor, float amount, string fromCurrency, string toCurrency, DateTime? date = null)
        {
            if (fromCurrency == toCurrency)
            {
                // Do nothing
            }
            else
            {
                var dateUtc = ConvertDateToUtc(date);

                var exchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(fromCurrency, toCurrency, dateUtc);

                amount = amount * exchangeRate;

                var transaction = new Transaction
                {
                    Investor = investor,
                    OperationId = Operation.Transfer,
                    Amount = amount,
                    Currency = toCurrency,
                    DateUtc = dateUtc,
                    ExchangeRate = exchangeRate
                };

                _unitOfWork.TransactionRepository.Add(transaction);
                _unitOfWork.SaveChanges();
            }

            return amount;
        }

        public float Balance(Investor investor, DateTime date)
        {
            var dateUtc = ConvertDateToUtc(date);

            // deposit - buy + sell - withdraw

            //     _unitOfWork.TransactionRepository.

            return 0f;
        }

        private DateTime ConvertDateToUtc(DateTime? date)
        {
            DateTime dateUtc;

            if (null == date || DateTime.MinValue == date.Value)
            {
                dateUtc = DateTime.Now;
            }
            else
            {
                dateUtc = date.Value;
            }

            return dateUtc.ToUniversalTime();
        }
    }
}
