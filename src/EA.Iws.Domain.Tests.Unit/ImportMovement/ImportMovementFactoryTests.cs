namespace EA.Iws.Domain.Tests.Unit.ImportMovement
{
    using System;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using Domain.ImportMovement;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using FakeItEasy;
    using TestHelpers.Helpers;
    using Xunit;

    public class ImportMovementFactoryTests
    {
        private readonly ImportNotificationAssessment assessment;
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly ImportMovementFactory factory;
        private readonly Guid notificationId = new Guid("76E2E791-CD7A-4BFE-8AD3-09E1A3B0305D");
        private readonly DateTime shipmentDate = new DateTime(2016, 11, 1);
        private readonly IImportMovementNumberValidator validator;

        public ImportMovementFactoryTests()
        {
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

        [Fact]
        public async Task NotificationNotConsented_Throws()
        {
            ObjectInstantiator<ImportNotificationAssessment>.SetProperty(x => x.Status,
                ImportNotificationStatus.FileClosed, assessment);

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(notificationId, 1, shipmentDate));
        }
    }
}