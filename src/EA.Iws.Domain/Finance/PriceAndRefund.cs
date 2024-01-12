namespace EA.Iws.Domain.Finance
{
    public class PriceAndRefund
    {
        public decimal Price { get; protected set; }

        public decimal PotentialRefund { get; protected set; }
    }
}