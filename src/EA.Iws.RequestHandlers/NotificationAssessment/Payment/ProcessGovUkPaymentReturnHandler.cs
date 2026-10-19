namespace EA.Iws.RequestHandlers.NotificationAssessment.Payment
{
    using Core.Shared;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Domain.Security;
    using EA.Iws.Api.Client.GovUkPay;
    using EA.Iws.Core.NotificationAssessment;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment.Payment;
    using System;
    using System.Threading.Tasks;

    internal class ProcessGovUkPaymentReturnHandler : IRequestHandler<ProcessGovUkPaymentReturn, ProcessGovUkPaymentReturnResult>
    {
        private readonly IGovUkPaySessionRepository paySessionRepository;
        private readonly INotificationApplicationAuthorization authorization;
        private readonly Transaction transaction;
        private readonly IwsContext context;
        private readonly IPayClient payClient;

        public ProcessGovUkPaymentReturnHandler(IGovUkPaySessionRepository paySessionRepository,
            INotificationApplicationAuthorization authorization,
            Transaction transaction,
            IwsContext context,
            IPayClient payClient)
        {
            this.paySessionRepository = paySessionRepository;
            this.authorization = authorization;
            this.transaction = transaction;
            this.context = context;
            this.payClient = payClient;
        }

        public async Task<ProcessGovUkPaymentReturnResult> HandleAsync(ProcessGovUkPaymentReturn message)
        {
            var session = await paySessionRepository.GetBySecureToken(message.SecureToken);

            if (session == null)
            {
                throw new InvalidOperationException("Payment session not found");
            }

            await authorization.EnsureAccessAsync(session.NotificationId);

            if (session.InFinalState)
            {
                return new ProcessGovUkPaymentReturnResult
                {
                    NotificationId = session.NotificationId,
                    Success = session.Status == "success",
                    PaymentReference = session.PaymentReference
                };
            }

            var payment = await payClient.GetPayment(session.PaymentId);

            session.UpdateStatus(payment.State.Status, payment.State.Finished, SystemTime.UtcNow);

            var success = payment.State.Finished && payment.State.Status == "success";

            if (success)
            {
                var transactionData = new NotificationTransactionData
                {
                    Date = SystemTime.UtcNow,
                    NotificationId = session.NotificationId,
                    Credit = session.Amount,
                    PaymentMethod = PaymentMethod.GovPay,
                    ReceiptNumber = session.PaymentReference,
                    Comments = string.Format("Paid online via GOV.UK Pay. Payment ID: {0}", session.PaymentId)
                };

                await transaction.Save(new NotificationTransaction(transactionData));
            }

            await context.SaveChangesAsync();

            return new ProcessGovUkPaymentReturnResult
            {
                NotificationId = session.NotificationId,
                Success = success,
                PaymentReference = session.PaymentReference
            };
        }
    }
}