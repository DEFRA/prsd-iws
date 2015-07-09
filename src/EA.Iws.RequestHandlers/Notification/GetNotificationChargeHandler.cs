namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using Domain.Notification;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationChargeHandler : IRequestHandler<GetNotificationCharge, decimal>
    {
        private readonly INotificationChargeCalculator chargeCalculator;

        public GetNotificationChargeHandler(INotificationChargeCalculator chargeCalculator)
        {
            this.chargeCalculator = chargeCalculator;
        }

        public async Task<decimal> HandleAsync(GetNotificationCharge message)
        {
            return await chargeCalculator.GetValue(message.NotificationId);
        }
    }
}