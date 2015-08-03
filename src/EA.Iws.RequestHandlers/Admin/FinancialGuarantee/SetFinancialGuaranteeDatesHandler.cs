namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using DataAccess;
    using Domain.FinancialGuarantee;
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
            if (!await context.NotificationApplications.AnyAsync(p => p.Id == message.NotificationId))
            {
                throw new InvalidOperationException(string.Format("Notification {0} does not exist.", message.NotificationId));
            }

            var financialGuarantee = await context.FinancialGuarantees.SingleOrDefaultAsync(na => na.NotificationApplicationId == message.NotificationId);

            if (financialGuarantee == null)
            {
                financialGuarantee = FinancialGuarantee.Create(message.NotificationId);
                context.FinancialGuarantees.Add(financialGuarantee);
            }

            if (message.ReceivedDate.HasValue)
            {
                if (financialGuarantee.Status == FinancialGuaranteeStatus.AwaitingApplication)
                {
                    financialGuarantee.Received(message.ReceivedDate.Value);
                }
                else if (financialGuarantee.ReceivedDate != message.ReceivedDate)
                {
                    financialGuarantee.UpdateReceivedDate(message.ReceivedDate.Value);
                }
            }

            if (message.CompletedDate.HasValue && financialGuarantee.ReceivedDate.HasValue)
            {
                if (financialGuarantee.Status == FinancialGuaranteeStatus.ApplicationReceived)
                {
                    financialGuarantee.Completed(message.CompletedDate.Value);
                }
                else if (financialGuarantee.CompletedDate.HasValue
                    && financialGuarantee.CompletedDate != message.CompletedDate)
                {
                    financialGuarantee.UpdateCompletedDate(message.CompletedDate.Value);
                }
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}
