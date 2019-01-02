namespace EA.Iws.DataAccess.Tests.Integration.SharedUser
{
    using System;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Domain.NotificationAssessment;
    using Domain.NotificationConsent;
    using Domain.Security;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using Repositories;
    using TestHelpers.Helpers;
    using Xunit;
    using NotificationApplicationFactory = TestHelpers.Helpers.NotificationApplicationFactory;

    public class NotificationApplicationRepositoryTests
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;
        private readonly Guid userId;

        public NotificationApplicationRepositoryTests()
        {
            userId = Guid.NewGuid();

            var userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(userId);

            context = new IwsContext(userContext, A.Fake<IEventDispatcher>());

            notificationApplicationAuthorization = A.Fake<INotificationApplicationAuthorization>();
        }

        [Fact]
        public async Task GetNotificationByIdChecksAuthorization()
        {
            var notification = NotificationApplicationFactory.Create(userId, NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);

            context.NotificationApplications.Add(notification);
            context.SaveChanges();

            var notificationId = notification.Id;

            var repository = new NotificationApplicationRepository(context,
                notificationApplicationAuthorization);

            await repository.GetById(notificationId);

            A.CallTo(() => notificationApplicationAuthorization.EnsureAccessAsync(notificationId)).MustHaveHappened();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetNotificationApplicationOverviewByIdChecksAuthorization()
        {
            var notification = NotificationApplicationFactory.Create(userId, NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);

            context.NotificationApplications.Add(notification);
            context.SaveChanges();

            var notificationId = notification.Id;

            var repository = new NotificationApplicationOverviewRepository(context,
                A.Fake<INotificationChargeCalculator>(),
                A.Fake<INotificationProgressService>(),
                notificationApplicationAuthorization);

            await repository.GetById(notificationId);

            A.CallTo(() => notificationApplicationAuthorization.EnsureAccessAsync(notificationId)).MustHaveHappened();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetNotificationDatesSummaryByIdChecksAuthorization()
        {
            var notification = NotificationApplicationFactory.Create(userId, NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);

            context.NotificationApplications.Add(notification);
            context.SaveChanges();

            var notificationId = notification.Id;

            var decisionRequiredBy = new DecisionRequiredBy(A.Fake<IDecisionRequiredByCalculator>(),
                A.Fake<IFacilityRepository>());
            var assessmentRepository = A.Fake<INotificationAssessmentRepository>();
            var applicationRepository = A.Fake<INotificationApplicationRepository>();
            var transactionCalculator = A.Fake<INotificationTransactionCalculator>();
            var assessment = new NotificationAssessment(notificationId);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.NotificationReceived, assessment);

            A.CallTo(() => assessmentRepository.GetByNotificationId(notificationId))
                .Returns(assessment);
            A.CallTo(() => applicationRepository.GetById(notificationId)).Returns(notification);

            var transaction = new NotificationTransaction(
                new NotificationTransactionData
                {
                    Credit = 1,
                    Date = new DateTime(2017, 1, 1),
                    NotificationId = notificationId,
                    PaymentMethod = PaymentMethod.Card
                });
            A.CallTo(() => transactionCalculator.PaymentReceivedDate(notificationId))
                .Returns(transaction);
            A.CallTo(() => transactionCalculator.LatestPayment(notificationId))
                .Returns(transaction);
            A.CallTo(() => transactionCalculator.IsPaymentComplete(notificationId)).Returns(true);

            var repository = new NotificationAssessmentDatesSummaryRepository(decisionRequiredBy,
                assessmentRepository,
                applicationRepository,
                transactionCalculator,
                notificationApplicationAuthorization);

            await repository.GetById(notificationId);

            A.CallTo(() => notificationApplicationAuthorization.EnsureAccessAsync(notificationId)).MustHaveHappened();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetNotificationAssessmentDecisionByIdChecksAuthorization()
        {
            var notification = NotificationApplicationFactory.Create(userId, NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);

            context.NotificationApplications.Add(notification);
            context.SaveChanges();

            var notificationId = notification.Id;

            var assessment = new NotificationAssessment(notificationId);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.NotificationReceived, assessment);
            var consent = new Consent(notificationId, new DateRange(DateTime.Now, DateTime.Now.AddDays(10)), "test", userId);

            context.NotificationAssessments.Add(assessment);
            context.Consents.Add(consent);
            context.SaveChanges();

            var repository = new NotificationAssessmentDecisionRepository(context,
                notificationApplicationAuthorization);

            await repository.GetByNotificationId(notificationId);

            A.CallTo(() => notificationApplicationAuthorization.EnsureAccessAsync(notificationId)).MustHaveHappened();

            context.DeleteOnCommit(assessment);
            context.DeleteOnCommit(consent);
            await context.SaveChangesAsync();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetNotificationAssessmentByIdChecksAuthorization()
        {
            var notification = NotificationApplicationFactory.Create(userId, NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);

            context.NotificationApplications.Add(notification);
            context.SaveChanges();

            var notificationId = notification.Id;

            var assessment = new NotificationAssessment(notificationId);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.NotificationReceived, assessment);

            context.NotificationAssessments.Add(assessment);
            context.SaveChanges();

            var repository = new NotificationAssessmentRepository(context,
                notificationApplicationAuthorization);

            await repository.GetByNotificationId(notificationId);

            A.CallTo(() => notificationApplicationAuthorization.EnsureAccessAsync(notificationId)).MustHaveHappened();

            context.DeleteOnCommit(assessment);
            await context.SaveChangesAsync();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetNotificationConsentByIdChecksAuthorization()
        {
            var notification = NotificationApplicationFactory.Create(userId, NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);

            context.NotificationApplications.Add(notification);
            context.SaveChanges();

            var notificationId = notification.Id;

            var consent = new Consent(notificationId, new DateRange(DateTime.Now, DateTime.Now.AddDays(10)), "test", userId);

            context.Consents.Add(consent);
            context.SaveChanges();

            var repository = new NotificationConsentRepository(context,
                notificationApplicationAuthorization);

            await repository.GetByNotificationId(notificationId);

            A.CallTo(() => notificationApplicationAuthorization.EnsureAccessAsync(notificationId)).MustHaveHappened();

            context.DeleteOnCommit(consent);
            await context.SaveChangesAsync();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetNotificationTransactionByIdChecksAuthorization()
        {
            var notification = NotificationApplicationFactory.Create(userId, NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);

            context.NotificationApplications.Add(notification);
            context.SaveChanges();

            var notificationId = notification.Id;

            var repository = new NotificationTransactionRepository(context,
                notificationApplicationAuthorization);

            await repository.GetTransactions(notificationId);

            A.CallTo(() => notificationApplicationAuthorization.EnsureAccessAsync(notificationId)).MustHaveHappened();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetNotificationNumberOfShipmentsHistoryChecksAuthorization()
        {
            var notification = NotificationApplicationFactory.Create(userId, NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);

            context.NotificationApplications.Add(notification);
            context.SaveChanges();

            var notificationId = notification.Id;

            var shipmentsHistory = new NumberOfShipmentsHistory(notificationId, 10, DateTime.Now);
            var shipmentsInfo = new ShipmentInfo(notificationId, new ShipmentPeriod(DateTime.Now, DateTime.Now.AddDays(10), true), 10, new ShipmentQuantity(1.0m, ShipmentQuantityUnits.Tonnes));

            context.NumberOfShipmentsHistories.Add(shipmentsHistory);
            context.ShipmentInfos.Add(shipmentsInfo);
            context.SaveChanges();

            var repository = new NumberOfShipmentsHistoryRepository(context,
                notificationApplicationAuthorization);

            await repository.GetOriginalNumberOfShipments(notificationId);

            await repository.GetCurrentNumberOfShipments(notificationId);

            await repository.GetLargestNumberOfShipments(notificationId);

            A.CallTo(() => notificationApplicationAuthorization.EnsureAccessAsync(notificationId))
                .MustHaveHappened(Repeated.Exactly.Times(4));

            context.DeleteOnCommit(shipmentsHistory);
            context.DeleteOnCommit(shipmentsInfo);
            await context.SaveChangesAsync();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetNotificationProducerByIdChecksAuthorization()
        {
            var notification = NotificationApplicationFactory.Create(userId, NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);

            context.NotificationApplications.Add(notification);
            context.SaveChanges();

            var notificationId = notification.Id;

            var producerCollection = new ProducerCollection(notificationId);
            producerCollection.AddProducer(ObjectFactory.CreateEmptyProducerBusiness(),
                ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyContact());

            context.Producers.Add(producerCollection);
            context.SaveChanges();

            var repository = new ProducerRepository(context,
                notificationApplicationAuthorization);

            await repository.GetByNotificationId(notificationId);

            A.CallTo(() => notificationApplicationAuthorization.EnsureAccessAsync(notificationId)).MustHaveHappened();
        
            context.DeleteOnCommit(producerCollection);
            await context.SaveChangesAsync();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetNotificationShipmentInfoByIdChecksAuthorization()
        {
            var notification = NotificationApplicationFactory.Create(userId, NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);

            context.NotificationApplications.Add(notification);
            context.SaveChanges();

            var notificationId = notification.Id;

            var shipmentsInfo = new ShipmentInfo(notificationId, new ShipmentPeriod(DateTime.Now, DateTime.Now.AddDays(10), true), 10, new ShipmentQuantity(1.0m, ShipmentQuantityUnits.Tonnes));
            var assessment = new NotificationAssessment(notificationId);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.NotificationReceived, assessment);
            var facilityCollection = new FacilityCollection(notificationId);
            facilityCollection.AddFacility(ObjectFactory.CreateEmptyProducerBusiness(),
                ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyContact());

            context.ShipmentInfos.Add(shipmentsInfo);
            context.NotificationAssessments.Add(assessment);
            context.Facilities.Add(facilityCollection);
            context.SaveChanges();

            var repository = new ShipmentInfoRepository(context,
                notificationApplicationAuthorization);

            await repository.GetByNotificationId(notificationId);

            await repository.GetIntendedShipmentDataByNotificationId(notificationId);

            A.CallTo(() => notificationApplicationAuthorization.EnsureAccessAsync(notificationId)).MustHaveHappened(Repeated.Exactly.Times(2));

            context.DeleteOnCommit(shipmentsInfo);
            context.DeleteOnCommit(assessment);
            context.DeleteOnCommit(facilityCollection);
            await context.SaveChangesAsync();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetNotificationTransportrouteByIdChecksAuthorization()
        {
            var notification = NotificationApplicationFactory.Create(userId, NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);

            context.NotificationApplications.Add(notification);
            context.SaveChanges();

            var notificationId = notification.Id;

            var repository = new TransportRouteRepository(context,
                notificationApplicationAuthorization);

            await repository.GetByNotificationId(notificationId);

            A.CallTo(() => notificationApplicationAuthorization.EnsureAccessAsync(notificationId)).MustHaveHappened();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetNotificationWasteDisposalByIdChecksAuthorization()
        {
            var notification = NotificationApplicationFactory.Create(userId, NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);

            context.NotificationApplications.Add(notification);
            context.SaveChanges();

            var notificationId = notification.Id;

            var repository = new WasteDisposalRepository(context,
                notificationApplicationAuthorization);

            await repository.GetByNotificationId(notificationId);

            A.CallTo(() => notificationApplicationAuthorization.EnsureAccessAsync(notificationId)).MustHaveHappened();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetNotificationWasteRecoveryByIdChecksAuthorization()
        {
            var notification = NotificationApplicationFactory.Create(userId, NotificationType.Recovery,
                UKCompetentAuthority.England, 20181);

            context.NotificationApplications.Add(notification);
            context.SaveChanges();

            var notificationId = notification.Id;

            var repository = new WasteRecoveryRepository(context,
                notificationApplicationAuthorization);

            await repository.GetByNotificationId(notificationId);

            A.CallTo(() => notificationApplicationAuthorization.EnsureAccessAsync(notificationId)).MustHaveHappened();

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }
    }
}
