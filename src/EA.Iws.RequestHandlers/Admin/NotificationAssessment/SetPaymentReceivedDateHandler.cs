namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class SetPaymentReceivedDateHandler : IRequestHandler<SetPaymentReceivedDate, bool>
    {
        private readonly IwsContext context;

        public SetPaymentReceivedDateHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetPaymentReceivedDate message)
        {
            var assessment =
                await
                    context.NotificationAssessments.SingleAsync(
                        p => p.NotificationApplicationId == message.NotificationId);

            assessment.SetPaymentReceived(message.PaymentReceivedDate);

            await context.SaveChangesAsync();

            return true;
        }
    }
}