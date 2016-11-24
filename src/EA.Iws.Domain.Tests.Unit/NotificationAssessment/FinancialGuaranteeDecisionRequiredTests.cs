namespace EA.Iws.Domain.Tests.Unit.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Core.NotificationAssessment;
    using Domain.FinancialGuarantee;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using TestHelpers.Helpers;
    using Xunit;

    public class FinancialGuaranteeDecisionRequiredTests
    {
        private static readonly Guid NotificationId = new Guid("F6D6B9D5-DE72-4C86-A1A7-627F281332EC");

        private readonly FinancialGuaranteeDecisionRequired service;
        private readonly INotificationAssessmentRepository assessmentRepository;
        private readonly IFinancialGuaranteeRepository financialGuaranteeRepository;

        public FinancialGuaranteeDecisionRequiredTests()
        {
            assessmentRepository = A.Fake<INotificationAssessmentRepository>();
            financialGuaranteeRepository = A.Fake<IFinancialGuaranteeRepository>();

            service = new FinancialGuaranteeDecisionRequired(assessmentRepository, financialGuaranteeRepository);
        }

        [Theory]
        [InlineData(FinancialGuaranteeStatus.AwaitingApplication)]
        [InlineData(FinancialGuaranteeStatus.ApplicationReceived)]
        [InlineData(FinancialGuaranteeStatus.ApplicationComplete)]
        public async Task NotificationStatusConsentedAndFGDecisionNotMade_ReturnsTrue(FinancialGuaranteeStatus status)
        {
            var assessment = new NotificationAssessment(NotificationId);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.Consented, assessment);

            var financialGuaranteeCollection = new FinancialGuaranteeCollection(NotificationId);
            var financialGuarantee = financialGuaranteeCollection.AddFinancialGuarantee(new DateTime(2015, 1, 1));
            ObjectInstantiator<FinancialGuarantee>.SetProperty(x => x.Status, status, financialGuarantee);

            A.CallTo(() => assessmentRepository.GetByNotificationId(NotificationId)).Returns(assessment);
            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(NotificationId)).Returns(financialGuaranteeCollection);

            var result = await service.Calculate(NotificationId);

            Assert.True(result);
        }

        [Theory]
        [InlineData(NotificationStatus.ConsentWithdrawn)]
        [InlineData(NotificationStatus.DecisionRequiredBy)]
        [InlineData(NotificationStatus.InAssessment)]
        [InlineData(NotificationStatus.InDetermination)]
        [InlineData(NotificationStatus.NotificationReceived)]
        [InlineData(NotificationStatus.NotSubmitted)]
        [InlineData(NotificationStatus.Objected)]
        [InlineData(NotificationStatus.ReadyToTransmit)]
        [InlineData(NotificationStatus.Submitted)]
        [InlineData(NotificationStatus.Transmitted)]
        [InlineData(NotificationStatus.Withdrawn)]
        public async Task NotificationNotConsented_ReturnsFalse(NotificationStatus status)
        {
            var assessment = new NotificationAssessment(NotificationId);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, status, assessment);

            var financialGuaranteeCollection = new FinancialGuaranteeCollection(NotificationId);
            var financialGuarantee = financialGuaranteeCollection.AddFinancialGuarantee(new DateTime(2015, 1, 1));
            ObjectInstantiator<FinancialGuarantee>.SetProperty(x => x.Status, FinancialGuaranteeStatus.ApplicationComplete, financialGuarantee);

            A.CallTo(() => assessmentRepository.GetByNotificationId(NotificationId)).Returns(assessment);
            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(NotificationId)).Returns(financialGuaranteeCollection);

            var result = await service.Calculate(NotificationId);

            Assert.False(result);
        }

        [Theory]
        [InlineData(FinancialGuaranteeStatus.Approved)]
        [InlineData(FinancialGuaranteeStatus.Refused)]
        [InlineData(FinancialGuaranteeStatus.Released)]
        public async Task FGDecisionMade_ReturnsFalse(FinancialGuaranteeStatus status)
        {
            var assessment = new NotificationAssessment(NotificationId);
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.Consented, assessment);

            var financialGuaranteeCollection = new FinancialGuaranteeCollection(NotificationId);
            var financialGuarantee = financialGuaranteeCollection.AddFinancialGuarantee(new DateTime(2015, 1, 1));
            ObjectInstantiator<FinancialGuarantee>.SetProperty(x => x.Status, status, financialGuarantee);

            A.CallTo(() => assessmentRepository.GetByNotificationId(NotificationId)).Returns(assessment);
            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(NotificationId)).Returns(financialGuaranteeCollection);

            var result = await service.Calculate(NotificationId);

            Assert.False(result);
        }
    }
}