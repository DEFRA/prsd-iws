namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationAssessment.Payment
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Domain.Security;
    using EA.Iws.Api.Client.GovUkPay;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using RequestHandlers.NotificationAssessment.Payment;
    using Requests.NotificationAssessment.Payment;
    using Xunit;

    public class CreateGovUkPaymentHandlerTests
    {
        private readonly CreateGovUkPaymentHandler handler;
        private readonly INotificationApplicationAuthorization authorization;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly INotificationTransactionCalculator transactionCalculator;
        private readonly IGovUkPaySessionRepository paySessionRepository;
        private readonly IGovUkPayConfiguration configuration;
        private readonly IPayClient payClient;
        private readonly TestIwsContext context;
        private readonly IUserContext userContext;

        private readonly Guid notificationId = Guid.NewGuid();
        private readonly Guid userId = Guid.NewGuid();
        private const string NotificationNumber = "GB1234567";

        public CreateGovUkPaymentHandlerTests()
        {
            authorization = A.Fake<INotificationApplicationAuthorization>();
            notificationApplicationRepository = A.Fake<INotificationApplicationRepository>();
            transactionCalculator = A.Fake<INotificationTransactionCalculator>();
            paySessionRepository = A.Fake<IGovUkPaySessionRepository>();
            configuration = A.Fake<IGovUkPayConfiguration>();
            payClient = A.Fake<IPayClient>();
            context = new TestIwsContext();
            userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(userId);
            A.CallTo(() => notificationApplicationRepository.GetNumber(notificationId)).Returns(NotificationNumber);
            A.CallTo(() => configuration.Description).Returns("Payment for {0}");
            A.CallTo(() => configuration.ReturnUrlFormat).Returns("https://example.com/{0}/{1}");
            A.CallTo(() => paySessionRepository.GetInProgressByNotificationId(notificationId)).Returns((GovUkPaySession)null);
            A.CallTo(() => payClient.CreatePayment(A<CreateCardPaymentRequest>.Ignored)).Returns(new CreatePaymentResult
            {
                PaymentId = "payment-id-default",
                NextUrl = "https://pay.example.com/next"
            });

            handler = new CreateGovUkPaymentHandler(authorization,
                notificationApplicationRepository,
                transactionCalculator,
                paySessionRepository,
                configuration,
                payClient,
                context,
                userContext);
        }

        [Fact]
        public async Task HandleAsync_EnsuresAccess()
        {
            A.CallTo(() => transactionCalculator.Balance(notificationId)).Returns(100m);

            await handler.HandleAsync(new CreateGovUkPayment(notificationId));

            A.CallTo(() => authorization.EnsureAccessAsync(notificationId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_BalanceIsZeroOrLess_ReturnsAlreadyPaid()
        {
            A.CallTo(() => transactionCalculator.Balance(notificationId)).Returns(0m);

            var result = await handler.HandleAsync(new CreateGovUkPayment(notificationId));

            Assert.True(result.AlreadyPaid);
            A.CallTo(() => payClient.CreatePayment(A<CreateCardPaymentRequest>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task HandleAsync_NoExistingSession_CreatesNewPayment()
        {
            A.CallTo(() => transactionCalculator.Balance(notificationId)).Returns(150m);
            A.CallTo(() => paySessionRepository.GetInProgressByNotificationId(notificationId)).Returns((GovUkPaySession)null);
            A.CallTo(() => payClient.CreatePayment(A<CreateCardPaymentRequest>.Ignored)).Returns(new CreatePaymentResult
            {
                PaymentId = "payment-id-1",
                NextUrl = "https://pay.example.com/next"
            });

            var result = await handler.HandleAsync(new CreateGovUkPayment(notificationId));

            Assert.Equal(NotificationNumber, result.NotificationNumber);
            Assert.Equal(150m, result.Amount);
            Assert.Equal("https://pay.example.com/next", result.NextUrl);
            A.CallTo(() => paySessionRepository.Add(A<GovUkPaySession>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_NoExistingSession_SavesChanges()
        {
            A.CallTo(() => transactionCalculator.Balance(notificationId)).Returns(150m);
            A.CallTo(() => paySessionRepository.GetInProgressByNotificationId(notificationId)).Returns((GovUkPaySession)null);
            A.CallTo(() => payClient.CreatePayment(A<CreateCardPaymentRequest>.Ignored)).Returns(new CreatePaymentResult
            {
                PaymentId = "payment-id-1",
                NextUrl = "https://pay.example.com/next"
            });

            await handler.HandleAsync(new CreateGovUkPayment(notificationId));

            Assert.Equal(1, context.SaveChangesCount);
        }

        [Fact]
        public async Task HandleAsync_ExistingSessionNotFinished_ReturnsPaymentInProgress()
        {
            A.CallTo(() => transactionCalculator.Balance(notificationId)).Returns(150m);

            var existingSession = new GovUkPaySession(notificationId, "payment-id-existing", NotificationNumber,
                Guid.NewGuid().ToString("N"), 150m, userId, DateTime.UtcNow);

            A.CallTo(() => paySessionRepository.GetInProgressByNotificationId(notificationId)).Returns(existingSession);
            A.CallTo(() => payClient.GetPayment("payment-id-existing")).Returns(new PaymentWithAllLinks
            {
                State = new PaymentState { Finished = false, Status = "created" }
            });

            var result = await handler.HandleAsync(new CreateGovUkPayment(notificationId));

            Assert.True(result.PaymentInProgress);
            A.CallTo(() => payClient.CreatePayment(A<CreateCardPaymentRequest>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task HandleAsync_ExistingSessionFinished_UpdatesStatusAndCreatesNewPayment()
        {
            A.CallTo(() => transactionCalculator.Balance(notificationId)).Returns(150m);

            var existingSession = new GovUkPaySession(notificationId, "payment-id-existing", NotificationNumber,
                Guid.NewGuid().ToString("N"), 150m, userId, DateTime.UtcNow);

            A.CallTo(() => paySessionRepository.GetInProgressByNotificationId(notificationId)).Returns(existingSession);
            A.CallTo(() => payClient.GetPayment("payment-id-existing")).Returns(new PaymentWithAllLinks
            {
                State = new PaymentState { Finished = true, Status = "failed" }
            });
            A.CallTo(() => payClient.CreatePayment(A<CreateCardPaymentRequest>.Ignored)).Returns(new CreatePaymentResult
            {
                PaymentId = "payment-id-new",
                NextUrl = "https://pay.example.com/next"
            });

            var result = await handler.HandleAsync(new CreateGovUkPayment(notificationId));

            Assert.False(result.PaymentInProgress);
            Assert.True(existingSession.InFinalState);
            A.CallTo(() => payClient.CreatePayment(A<CreateCardPaymentRequest>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_CreatesPaymentWithRequestMatchingBalanceAndNotificationNumber()
        {
            A.CallTo(() => transactionCalculator.Balance(notificationId)).Returns(99.5m);
            A.CallTo(() => paySessionRepository.GetInProgressByNotificationId(notificationId)).Returns((GovUkPaySession)null);
            A.CallTo(() => payClient.CreatePayment(A<CreateCardPaymentRequest>.Ignored)).Returns(new CreatePaymentResult
            {
                PaymentId = "payment-id-1",
                NextUrl = "https://pay.example.com/next"
            });

            await handler.HandleAsync(new CreateGovUkPayment(notificationId));

            A.CallTo(() => payClient.CreatePayment(A<CreateCardPaymentRequest>.That.Matches(
                r => r.Amount == 9950 && r.Reference == NotificationNumber))).MustHaveHappenedOnceExactly();
        }
    }
}