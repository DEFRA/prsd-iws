namespace EA.Iws.Api.Client.GovUkPay
{
    using System.Threading.Tasks;

    public interface IPayClient
    {
        Task<CreatePaymentResult> CreatePayment(CreateCardPaymentRequest request);

        Task<PaymentWithAllLinks> GetPayment(string paymentId);
    }
}