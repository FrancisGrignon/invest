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

        public void Buy(Investor investor, Stock stock, decimal quantity)
        {
            // Check if enough money!

            var amount = quantity * stock.Value;

            var transaction = new Transaction
            {
                Investor = investor,
                OperationId = Operation.Buy,
                Stock = stock,
                Quantity = quantity,
                Amount = amount,
                Currency = stock.Currency
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
            }

            investment.Quantity = investment.Quantity + quantity;

            _unitOfWork.InvestmentRepository.Update(investment);            
        }

        public void Sell(Investor investor, Stock stock, decimal quantity)
        {
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
                Currency = stock.Currency
            };            

            _unitOfWork.TransactionRepository.Add(transaction);

            // Reduce stocks
            investment.Quantity = investment.Quantity - quantity;

            _unitOfWork.InvestmentRepository.Update(investment);
        }

        public void Deposit(Investor investor, decimal amount, string currency)
        {
            var transaction = new Transaction
            {
                Investor = investor,
                OperationId = Operation.Deposit,
                Amount = amount,
                Currency = currency
            };

            _unitOfWork.TransactionRepository.Add(transaction);
        }

        public void Withdraw(Investor investor, decimal amount, string currency)
        {
            // Check if enough money

            var transaction = new Transaction
            {
                Investor = investor,
                OperationId = Operation.Withdraw,
                Amount = amount,
                Currency = currency
            };

            _unitOfWork.TransactionRepository.Add(transaction);
        }

        public void Dividend(Investor investor, Stock stock, decimal amount)
        {
            var transaction = new Transaction
            {
                Investor = investor,
                OperationId = Operation.Dividend,
                Stock = stock,
                Amount = amount,
                Currency = stock.Currency
            };

            _unitOfWork.TransactionRepository.Add(transaction);
        }

        public void Split(Investor investor, Stock stock, int ratio)
        {

        }

        public void Merge(Investor investor, Stock stock, int ratio)
        {

        }

        public void Transfer(Investor investor, decimal amount, string fromCurrency, string toDestination)
        {
            Withdraw(investor, amount, fromCurrency);

            var forex = _unitOfWork.ForexRepository.GetByCurrency(fromCurrency);

            amount = amount * forex.ExchangeRate;

            Deposit(investor, amount, toDestination);
        }
    }
}
