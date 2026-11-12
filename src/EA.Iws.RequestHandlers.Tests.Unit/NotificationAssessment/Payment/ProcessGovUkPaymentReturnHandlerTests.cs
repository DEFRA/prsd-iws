namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationAssessment.Payment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using Domain.Security;
    using EA.Iws.Api.Client.GovUkPay;
    using FakeItEasy;
    using RequestHandlers.NotificationAssessment.Payment;
    using Requests.NotificationAssessment.Payment;
    using Xunit;

    public class ProcessGovUkPaymentReturnHandlerTests
    {
        private readonly ProcessGovUkPaymentReturnHandler handler;
        private readonly IGovUkPaySessionRepository paySessionRepository;
        private readonly INotificationApplicationAuthorization authorization;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly INotificationTransactionRepository transactionRepository;
        private readonly INotificationTransactionCalculator transactionCalculator;
        private readonly Transaction transaction;
        private readonly TestIwsContext context;
        private readonly IPayClient payClient;

        private readonly Guid notificationId = Guid.NewGuid();
        private readonly Guid userId = Guid.NewGuid();
        private readonly string secureToken = Guid.NewGuid().ToString("N");

        public ProcessGovUkPaymentReturnHandlerTests()
        {
            paySessionRepository = A.Fake<IGovUkPaySessionRepository>();
            authorization = A.Fake<INotificationApplicationAuthorization>();
            notificationAssessmentRepository = A.Fake<INotificationAssessmentRepository>();
            transactionRepository = A.Fake<INotificationTransactionRepository>();
            transactionCalculator = A.Fake<INotificationTransactionCalculator>();
            context = new TestIwsContext();
            payClient = A.Fake<IPayClient>();

            A.CallTo(() => notificationAssessmentRepository.GetByNotificationId(notificationId))
                .Returns(new NotificationAssessment(notificationId));
            A.CallTo(() => transactionRepository.GetTransactions(notificationId))
                .Returns(new List<NotificationTransaction>());
            A.CallTo(() => transactionCalculator.RefundLimit(notificationId)).Returns(decimal.MaxValue);
            A.CallTo(() => transactionCalculator.Balance(notificationId)).Returns(125.50m);

            transaction = new Transaction(notificationAssessmentRepository, transactionRepository, transactionCalculator);

            handler = new ProcessGovUkPaymentReturnHandler(paySessionRepository, authorization, transaction, context, payClient);
        }

        private GovUkPaySession GetSession(bool inFinalState = false, string status = "created")
        {
            var session = new GovUkPaySession(notificationId, "payment-id", "GB1234567", secureToken, 125.50m, userId, DateTime.UtcNow);

            if (inFinalState)
            {
                session.UpdateStatus(status, true, DateTime.UtcNow);
            }

            return session;
        }

        [Fact]
        public async Task HandleAsync_SessionNotFound_ThrowsInvalidOperationException()
        {
            A.CallTo(() => paySessionRepository.GetBySecureToken(secureToken)).Returns((GovUkPaySession)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.HandleAsync(new ProcessGovUkPaymentReturn(secureToken)));
        }

        [Fact]
        public async Task HandleAsync_SessionFound_EnsuresAccess()
        {
            var session = GetSession();
            A.CallTo(() => paySessionRepository.GetBySecureToken(secureToken)).Returns(session);
            A.CallTo(() => payClient.GetPayment("payment-id")).Returns(new PaymentWithAllLinks
            {
                State = new PaymentState { Finished = true, Status = "success" }
            });

            await handler.HandleAsync(new ProcessGovUkPaymentReturn(secureToken));

            A.CallTo(() => authorization.EnsureAccessAsync(notificationId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_SessionAlreadyInFinalState_DoesNotCallPayClient()
        {
            var session = GetSession(inFinalState: true, status: "success");
            A.CallTo(() => paySessionRepository.GetBySecureToken(secureToken)).Returns(session);

            var result = await handler.HandleAsync(new ProcessGovUkPaymentReturn(secureToken));

            Assert.True(result.Success);
            A.CallTo(() => payClient.GetPayment(A<string>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task HandleAsync_PaymentSuccessful_SavesTransaction()
        {
            var session = GetSession();
            A.CallTo(() => paySessionRepository.GetBySecureToken(secureToken)).Returns(session);
            A.CallTo(() => payClient.GetPayment("payment-id")).Returns(new PaymentWithAllLinks
            {
                State = new PaymentState { Finished = true, Status = "success" }
            });

            var result = await handler.HandleAsync(new ProcessGovUkPaymentReturn(secureToken));

            Assert.True(result.Success);
            A.CallTo(() => transactionRepository.Add(A<NotificationTransaction>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HandleAsync_PaymentFailed_DoesNotSaveTransaction()
        {
            var session = GetSession();
            A.CallTo(() => paySessionRepository.GetBySecureToken(secureToken)).Returns(session);
            A.CallTo(() => payClient.GetPayment("payment-id")).Returns(new PaymentWithAllLinks
            {
                State = new PaymentState { Finished = true, Status = "failed" }
            });

            var result = await handler.HandleAsync(new ProcessGovUkPaymentReturn(secureToken));

            Assert.False(result.Success);
            A.CallTo(() => transactionRepository.Add(A<NotificationTransaction>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task HandleAsync_UpdatesSessionStatus()
        {
            var session = GetSession();
            A.CallTo(() => paySessionRepository.GetBySecureToken(secureToken)).Returns(session);
            A.CallTo(() => payClient.GetPayment("payment-id")).Returns(new PaymentWithAllLinks
            {
                State = new PaymentState { Finished = true, Status = "success" }
            });

            await handler.HandleAsync(new ProcessGovUkPaymentReturn(secureToken));

            Assert.True(session.InFinalState);
            Assert.Equal("success", session.Status);
        }

        [Fact]
        public async Task HandleAsync_SavesChanges()
        {
            var session = GetSession();
            A.CallTo(() => paySessionRepository.GetBySecureToken(secureToken)).Returns(session);
            A.CallTo(() => payClient.GetPayment("payment-id")).Returns(new PaymentWithAllLinks
            {
                State = new PaymentState { Finished = true, Status = "success" }
            });

            await handler.HandleAsync(new ProcessGovUkPaymentReturn(secureToken));

            Assert.Equal(1, context.SaveChangesCount);
        }

        [Fact]
        public async Task HandleAsync_ReturnsPaymentReferenceFromSession()
        {
            var session = GetSession();
            A.CallTo(() => paySessionRepository.GetBySecureToken(secureToken)).Returns(session);
            A.CallTo(() => payClient.GetPayment("payment-id")).Returns(new PaymentWithAllLinks
            {
                State = new PaymentState { Finished = true, Status = "success" }
            });

            var result = await handler.HandleAsync(new ProcessGovUkPaymentReturn(secureToken));

            Assert.Equal("GB1234567", result.PaymentReference);
        }
    }
}