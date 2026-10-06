namespace EA.Iws.RequestHandlers.NotificationAssessment.Payment
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Domain.Security;
    using EA.Iws.Api.Client.GovUkPay;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment.Payment;

    internal class CreateGovUkPaymentHandler : IRequestHandler<CreateGovUkPayment, CreateGovUkPaymentResult>
    {
        private readonly INotificationApplicationAuthorization authorization;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly INotificationTransactionCalculator transactionCalculator;
        private readonly IGovUkPaySessionRepository paySessionRepository;
        private readonly IGovUkPayConfiguration configuration;
        private readonly IPayClient payClient;
        private readonly IwsContext context;

        public CreateGovUkPaymentHandler(INotificationApplicationAuthorization authorization,
            INotificationApplicationRepository notificationApplicationRepository,
            INotificationTransactionCalculator transactionCalculator,
            IGovUkPaySessionRepository paySessionRepository,
            IGovUkPayConfiguration configuration,
            IPayClient payClient,
            IwsContext context)
        {
            this.authorization = authorization;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.transactionCalculator = transactionCalculator;
            this.paySessionRepository = paySessionRepository;
            this.configuration = configuration;
            this.payClient = payClient;
            this.context = context;
        }

        public async Task<CreateGovUkPaymentResult> HandleAsync(CreateGovUkPayment message)
        {
            await authorization.EnsureAccessAsync(message.NotificationId);

            var balance = await transactionCalculator.Balance(message.NotificationId);

            if (balance <= 0)
            {
                return new CreateGovUkPaymentResult { AlreadyPaid = true };
            }

            var existingSession = await paySessionRepository.GetInProgressByNotificationId(message.NotificationId);

            if (existingSession != null)
            {
                var existingPayment = await payClient.GetPayment(existingSession.PaymentId);

                if (!existingPayment.State.Finished)
                {
                    return new CreateGovUkPaymentResult { PaymentInProgress = true };
                }

                existingSession.UpdateStatus(existingPayment.State.Status, existingPayment.State.Finished, DateTime.UtcNow);

                await context.SaveChangesAsync();
            }

            var notificationNumber = await notificationApplicationRepository.GetNumber(message.NotificationId);

            var secureToken = Guid.NewGuid().ToString("N");
            var description = string.Format(configuration.Description, notificationNumber);
            var returnUrl = string.Format(configuration.ReturnUrlFormat, message.NotificationId, secureToken);

            var createPaymentRequest = new CreateCardPaymentRequest
            {
                Amount = (int)(balance * 100),
                Reference = notificationNumber,
                Description = description,
                ReturnUrl = returnUrl
            };

            var paymentResult = await payClient.CreatePayment(createPaymentRequest);

            var session = new GovUkPaySession(message.NotificationId,
                paymentResult.PaymentId,
                notificationNumber,
                secureToken,
                balance,
                DateTime.UtcNow);

            paySessionRepository.Add(session);

            await context.SaveChangesAsync();

            return new CreateGovUkPaymentResult
            {
                NotificationNumber = notificationNumber,
                SecureToken = secureToken,
                Amount = balance,
                Description = description,
                NextUrl = paymentResult.NextUrl
            };
        }
    }
}