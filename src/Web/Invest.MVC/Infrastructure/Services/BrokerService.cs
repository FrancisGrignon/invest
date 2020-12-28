using System;

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

        public Investment Buy(Investor investor, Stock stock, decimal quantity, DateTime? date = null)
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
                DateUtc = dateUtc
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

            return investment;
        }

        public decimal Sell(Investor investor, Stock stock, decimal quantity, DateTime? date = null)
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

            // Check if how stock
            var amount = quantity * stock.Value;

            // Add money
            var transaction = new Transaction
            {
                Investor = investor,
                OperationId = Operation.Sell,
                Stock = stock,
                Quantity = quantity,
                Amount = amount,
                Currency = stock.Currency,
                DateUtc = dateUtc
            };            

            _unitOfWork.TransactionRepository.Add(transaction);

            // Reduce stocks
            investment.Quantity = investment.Quantity - quantity;

            _unitOfWork.InvestmentRepository.Update(investment);

            return amount;
        }

        public void Deposit(Investor investor, decimal amount, string currency, DateTime? date = null)
        {
            var dateUtc = ConvertDateToUtc(date);

            var transaction = new Transaction
            {
                Investor = investor,
                OperationId = Operation.Deposit,
                Amount = amount,
                Currency = currency,
                DateUtc = dateUtc
            };

            _unitOfWork.TransactionRepository.Add(transaction);
        }

        public void Withdraw(Investor investor, decimal amount, string currency, DateTime? date = null)
        {
            var dateUtc = ConvertDateToUtc(date);

            // Check if enough money

            var transaction = new Transaction
            {
                Investor = investor,
                OperationId = Operation.Withdraw,
                Amount = amount,
                Currency = currency,
                DateUtc = dateUtc
            };

            _unitOfWork.TransactionRepository.Add(transaction);
        }

        public void Dividend(Investor investor, Stock stock, decimal amount, DateTime? date = null)
        {
            var dateUtc = ConvertDateToUtc(date);

            var transaction = new Transaction
            {
                Investor = investor,
                OperationId = Operation.Dividend,
                Stock = stock,
                Amount = amount,
                Currency = stock.Currency,
                DateUtc = dateUtc
            };

            _unitOfWork.TransactionRepository.Add(transaction);
        }

        public void Split(Investor investor, Stock stock, int ratio, DateTime? date = null)
        {

        }

        public void Merge(Investor investor, Stock stock, int ratio, DateTime? date = null)
        {

        }

        public decimal Transfer(Investor investor, decimal amount, string fromCurrency, string toDestination, DateTime? date = null)
        {
            var dateUtc = ConvertDateToUtc(date);

            Withdraw(investor, amount, fromCurrency, date);

            var exchangeRate = _unitOfWork.ForexRepository.GetExchangeRate(fromCurrency, toDestination, dateUtc);

            amount = amount * exchangeRate;

            Deposit(investor, amount, toDestination, date);

            return amount;
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
