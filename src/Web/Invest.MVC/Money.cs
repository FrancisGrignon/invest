namespace Invest.MVC
{
    public struct Money
    {
        public Money(float amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public float Amount { get; }

        public string Currency { get; }

        public override string ToString() => $"({Amount} {Currency})";

        #region Operator +, -

        public static Money operator +(Money left, Money right)
        {
            return new Money(left.Amount + right.Amount, left.Currency);
        }

        public static Money operator +(Money left, float right)
        {
            return new Money(left.Amount + right, left.Currency);
        }

        public static Money operator -(Money left, Money right)
        {
            return new Money(left.Amount - right.Amount, left.Currency);
        }

        #endregion
    }
}
