namespace EA.Iws.Domain.Tests.Unit.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using Xunit;

    public class DecisionRequiredByTests
    {
        private readonly IDecisionRequiredByCalculator decisionRequiredByCalculator;
        private readonly IFacilityRepository facilityRepository;
        private readonly DecisionRequiredBy decisionRequiredBy;
        private readonly NotificationApplication application;
        private readonly NotificationAssessment assessment;
        private readonly DateTime? decisionRequiredByDateWhenPopulatedInDB;
        private readonly DateTime? decisionRequiredByDateWhenNotPopulatedInDB;
        private readonly DateTime? acknowledgedDateWhenHasValue;
        private readonly DateTime? acknowledgedDateWhenHasNoValue;
        private readonly DateTime? decisionRequiredByDateWhenAcknowledgeDateHasNoValue;
        private readonly DateTime? decisionRequiredByDateWhenCalculated;

        public DecisionRequiredByTests()
        {
            var applicationId = new Guid();
            var userId = new Guid();
            var notificationType = NotificationType.Recovery;
            var competentAuthority = UKCompetentAuthority.England;
            var notificationNumber = 1;

            decisionRequiredByCalculator = A.Fake<IDecisionRequiredByCalculator>();
            facilityRepository = A.Fake<IFacilityRepository>();
            assessment = new NotificationAssessment(applicationId);
            application = new NotificationApplication(userId, notificationType, competentAuthority, notificationNumber);

            decisionRequiredBy = new DecisionRequiredBy(decisionRequiredByCalculator, facilityRepository);

            decisionRequiredByDateWhenPopulatedInDB = new DateTime(2019, 1, 1);
            decisionRequiredByDateWhenNotPopulatedInDB = null;
            acknowledgedDateWhenHasValue = new DateTime(2019, 2, 2);
            acknowledgedDateWhenHasNoValue = null;
            decisionRequiredByDateWhenAcknowledgeDateHasNoValue = null;
            decisionRequiredByDateWhenCalculated = new DateTime(2019, 3, 3);
        }

        [Fact]
        public async Task DecisionRequiredByDate_IsDateRetrivedFromDatabase()
        {
            assessment.Dates.AcknowledgedDate = acknowledgedDateWhenHasValue;
            assessment.Dates.DecisionRequiredByDate = decisionRequiredByDateWhenPopulatedInDB;

            var decisionRequiredByDate = await decisionRequiredBy.GetDecisionRequiredByDate(application, assessment);

            Assert.Equal(decisionRequiredByDateWhenPopulatedInDB, decisionRequiredByDate);
        }

        [Fact]
        public async Task DecisionRequiredByDate_AcknowledgedDateHasNoValue()
        {
            assessment.Dates.AcknowledgedDate = acknowledgedDateWhenHasNoValue;
            assessment.Dates.DecisionRequiredByDate = decisionRequiredByDateWhenNotPopulatedInDB;

            var decisionRequiredByDate = await decisionRequiredBy.GetDecisionRequiredByDate(application, assessment);

            Assert.Equal(decisionRequiredByDateWhenAcknowledgeDateHasNoValue, decisionRequiredByDate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DecisionRequiredByDate_DateCalculated(bool areFacilitiesPreconsented)
        {
            assessment.Dates.AcknowledgedDate = acknowledgedDateWhenHasValue;
            assessment.Dates.DecisionRequiredByDate = decisionRequiredByDateWhenNotPopulatedInDB;

            A.CallTo(() => facilityRepository.GetByNotificationId(application.Id))
                .Returns(GetFacilityCollectionTestData(areFacilitiesPreconsented));

            A.CallTo(() => decisionRequiredByCalculator.Get(areFacilitiesPreconsented, assessment.Dates.AcknowledgedDate.Value, application.CompetentAuthority))
                .Returns(decisionRequiredByDateWhenCalculated.Value);

           var decisionRequiredByDate = await decisionRequiredBy.GetDecisionRequiredByDate(application, assessment);

            Assert.Equal(decisionRequiredByDateWhenCalculated, decisionRequiredByDate);
        }

        private FacilityCollection GetFacilityCollectionTestData(bool areFacilitiesPreconsented)
        {
            var notificationId = new Guid();
            return new FacilityCollection(notificationId)
            {
                AllFacilitiesPreconsented = areFacilitiesPreconsented
            };
        }
    }
}
