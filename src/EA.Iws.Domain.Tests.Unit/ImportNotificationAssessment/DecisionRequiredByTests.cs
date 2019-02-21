namespace EA.Iws.Domain.Tests.Unit.ImportNotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Shared;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment.Decision;
    using FakeItEasy;
    using Xunit;

    public class DecisionRequiredByTests
    {
        private readonly IDecisionRequiredByCalculator decisionRequiredByCalculator;
        private readonly IFacilityRepository facilityRepository;
        private readonly IImportNotificationRepository importNotificationRepository;
        private readonly DecisionRequiredBy decisionRequiredBy;
        private readonly ImportNotificationAssessment assessment;
        private readonly DateTime? decisionRequiredByDateWhenPopulatedInDB;
        private readonly DateTime? decisionRequiredByDateWhenNotPopulatedInDB;
        private readonly DateTime? acknowledgedDateWhenHasValue;
        private readonly DateTime? acknowledgedDateWhenHasNoValue;
        private readonly DateTime? decisionRequiredByDateWhenAcknowledgeDateHasNoValue;
        private readonly DateTime? decisionRequiredByDateWhenCalculated;
        private readonly Guid notificationId;
        private readonly UKCompetentAuthority competentAuthority;

        public DecisionRequiredByTests()
        {
            notificationId = new Guid();
            competentAuthority = UKCompetentAuthority.England;

            decisionRequiredByCalculator = A.Fake<IDecisionRequiredByCalculator>();
            facilityRepository = A.Fake<IFacilityRepository>();
            importNotificationRepository = A.Fake<IImportNotificationRepository>();
            assessment = new ImportNotificationAssessment(notificationId);

            decisionRequiredBy = new DecisionRequiredBy(importNotificationRepository, facilityRepository, decisionRequiredByCalculator);

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

            var decisionRequiredByDate = await decisionRequiredBy.GetDecisionRequiredByDate(assessment);

            Assert.Equal(decisionRequiredByDateWhenPopulatedInDB, decisionRequiredByDate);
        }

        [Fact]
        public async Task DecisionRequiredByDate_AcknowledgedDateHasNoValue()
        {
            assessment.Dates.AcknowledgedDate = acknowledgedDateWhenHasNoValue;
            assessment.Dates.DecisionRequiredByDate = decisionRequiredByDateWhenNotPopulatedInDB;

            var decisionRequiredByDate = await decisionRequiredBy.GetDecisionRequiredByDate(assessment);

            Assert.Equal(decisionRequiredByDateWhenAcknowledgeDateHasNoValue, decisionRequiredByDate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DecisionRequiredByDate_DateCalculated(bool areFacilitiesPreconsented)
        {
            assessment.Dates.AcknowledgedDate = acknowledgedDateWhenHasValue;
            assessment.Dates.DecisionRequiredByDate = decisionRequiredByDateWhenNotPopulatedInDB;

            A.CallTo(() => importNotificationRepository.Get(notificationId))
                .Returns(GetImportNotificationTestData());

            A.CallTo(() => facilityRepository.GetByNotificationId(notificationId))
                .Returns(GetFacilityCollectionTestData(areFacilitiesPreconsented));

            A.CallTo(() => decisionRequiredByCalculator.Get(areFacilitiesPreconsented, assessment.Dates.AcknowledgedDate.Value, competentAuthority))
                .Returns(decisionRequiredByDateWhenCalculated.Value);

           var decisionRequiredByDate = await decisionRequiredBy.GetDecisionRequiredByDate(assessment);

            Assert.Equal(decisionRequiredByDateWhenCalculated, decisionRequiredByDate);
        }

        private ImportNotification GetImportNotificationTestData()
        {
            var notificationType = NotificationType.Recovery;
            var notificationNumber = "1";

            return new ImportNotification(notificationType, competentAuthority, notificationNumber);
        }

        private FacilityCollection GetFacilityCollectionTestData(bool areFacilitiesPreconsented)
        {
            return new FacilityCollection(notificationId, GetFacilityListTestData(), areFacilitiesPreconsented);
        }

        private FacilityList GetFacilityListTestData()
        {
            var address = new Address("AddressLine1", "AddressLine2", "TownOrCity", "PostalCode", new Guid());
            var contact = new Contact("Name", new PhoneNumber("01234567890"), new EmailAddress("email@address.com"));
            var facilityList = new List<Facility>()
            {
                new Facility("businessName", BusinessType.SoleTrader, "RegNumber", address, contact, true)
            };
            return new FacilityList(facilityList);
        }
    }
}
