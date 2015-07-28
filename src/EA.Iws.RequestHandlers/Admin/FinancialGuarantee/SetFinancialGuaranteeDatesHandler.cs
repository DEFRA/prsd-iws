namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class SetFinancialGuaranteeDatesHandler : IRequestHandler<SetFinancialGuaranteeDates, bool>
    {
        private readonly IwsContext context;

        public SetFinancialGuaranteeDatesHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetFinancialGuaranteeDates message)
        {
            var notificationAssessment = await context.NotificationAssessments.SingleAsync(na => na.NotificationApplicationId == message.NotificationId);

            if (message.ReceivedDate.HasValue)
            {
                if (notificationAssessment.FinancialGuarantee.Status == FinancialGuaranteeStatus.AwaitingApplication)
                {
                    notificationAssessment.FinancialGuarantee.Received(message.ReceivedDate.Value);
                }
                else if (notificationAssessment.FinancialGuarantee.ReceivedDate != message.ReceivedDate)
                {
                    notificationAssessment.FinancialGuarantee.UpdateReceivedDate(message.ReceivedDate.Value);
                }
            }

            if (message.CompletedDate.HasValue && notificationAssessment.FinancialGuarantee.ReceivedDate.HasValue)
            {
                if (notificationAssessment.FinancialGuarantee.Status == FinancialGuaranteeStatus.ApplicationReceived)
                {
                notificationAssessment.FinancialGuarantee.Completed(message.CompletedDate.Value);
                }
                else if (notificationAssessment.FinancialGuarantee.CompletedDate.HasValue 
                    && notificationAssessment.FinancialGuarantee.CompletedDate != message.CompletedDate)
                {
                    notificationAssessment.FinancialGuarantee.UpdateCompletedDate(message.CompletedDate.Value);
                }
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}
