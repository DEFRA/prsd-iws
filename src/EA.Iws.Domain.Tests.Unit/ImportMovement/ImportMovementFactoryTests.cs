namespace EA.Iws.Domain.Tests.Unit.ImportMovement
{
    using Core.ImportNotificationAssessment;
    using Domain.ImportMovement;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using FakeItEasy;
    using Prsd.Core;
    using System;
    using System.Threading.Tasks;
    using TestHelpers.Helpers;
    using Xunit;

    public class ImportMovementFactoryTests : IDisposable
    {
        private readonly ImportNotificationAssessment assessment;
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly ImportMovementFactory factory;
        private readonly Guid notificationId = new Guid("76E2E791-CD7A-4BFE-8AD3-09E1A3B0305D");
        private readonly DateTime shipmentDate = new DateTime(2016, 11, 1);
        private readonly IImportMovementNumberValidator validator;
        private static readonly DateTime Today = new DateTime(2017, 07, 19);
        private static readonly DateTime PastDate = Today.AddDays(-2);
        private static readonly DateTime FutureDate = Today.AddDays(2);
        private static readonly DateTime FutureSixtyDate = Today.AddDays(60);
        private static readonly DateTime FutureSixtyOneDate = Today.AddDays(61);

        public ImportMovementFactoryTests()
        {
            SystemTime.Freeze(Today);
            assessmentRepository = A.Fake<IImportNotificationAssessmentRepository>();
            validator = A.Fake<IImportMovementNumberValidator>();

            A.CallTo(() => validator.Validate(notificationId, A<int>.Ignored))
                .Returns(true);

            assessment = new ImportNotificationAssessment(notificationId);
            ObjectInstantiator<ImportNotificationAssessment>.SetProperty(x => x.Status,
                ImportNotificationStatus.Consented, assessment);

            A.CallTo(() => assessmentRepository.GetByNotification(notificationId))
                .Returns(assessment);

            factory = new ImportMovementFactory(validator, assessmentRepository);
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        [Fact]
        public async Task NotificationNotConsented_Throws()
        {
            ObjectInstantiator<ImportNotificationAssessment>.SetProperty(x => x.Status,
                ImportNotificationStatus.FileClosed, assessment);

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(notificationId, 1, shipmentDate, null));
        }

        [Fact]
        public async Task PrenotificationDateCanBeInThePast()
        {
            A.CallTo(() => validator.Validate(notificationId, A<int>.Ignored))
                .Returns(true);

            var result = await factory.Create(notificationId, 51, Today, PastDate);

            Assert.Equal(Today, result.ActualShipmentDate);
        }

        [Fact]
        public async Task PrenotificationDateInfuture_Throws()
        {
            A.CallTo(() => validator.Validate(notificationId, A<int>.Ignored))
                .Returns(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(notificationId, 52, Today, FutureDate));
        }

        [Fact]
        public async Task PrenotificationDateCanBeToday()
        {
            A.CallTo(() => validator.Validate(notificationId, A<int>.Ignored))
                .Returns(true);

            var result = await factory.Create(notificationId, 53, Today, Today);

            Assert.Equal(Today, result.ActualShipmentDate);
        }

        [Fact]
        public async Task ActualDateCanBeInThePast()
        {
            A.CallTo(() => validator.Validate(notificationId, A<int>.Ignored))
                .Returns(true);

            var result = await factory.Create(notificationId, 54, PastDate, PastDate);

            Assert.Equal(PastDate, result.ActualShipmentDate);
        }

        [Fact]
        public async Task ActualDateCanBeToday()
        {
            A.CallTo(() => validator.Validate(notificationId, A<int>.Ignored))
                .Returns(true);

            var result = await factory.Create(notificationId, 55, Today, PastDate);

            Assert.Equal(Today, result.ActualShipmentDate);
        }

        [Fact]
        public async Task ActualDateBeforePrenotificationDate_Throws()
        {
            A.CallTo(() => validator.Validate(notificationId, A<int>.Ignored))
                .Returns(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(notificationId, 56, PastDate, Today));
        }

        [Fact]
        public async Task ActualDateEqualToSixtydaysPrenotificationDate_Throws()
        {
            A.CallTo(() => validator.Validate(notificationId, A<int>.Ignored))
                .Returns(true);
            var result = await factory.Create(notificationId, 57, FutureSixtyDate, Today);

            Assert.Equal(FutureSixtyDate, result.ActualShipmentDate);
        }

        [Fact]
        public async Task ActualDateGreaterThanSixtydaysPrenotificationDate_Throws()
        {
            A.CallTo(() => validator.Validate(notificationId, A<int>.Ignored))
                .Returns(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(notificationId, 58, FutureSixtyOneDate, Today));
        }
    }
}