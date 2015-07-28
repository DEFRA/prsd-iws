namespace EA.Iws.RequestHandlers.Tests.Unit.Admin
{
    using System;
    using System.Threading.Tasks;
    using Core.Admin;
    using DataAccess;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using Helpers;
    using RequestHandlers.Admin;
    using Requests.Admin;
    using TestHelpers.Helpers;
    using Xunit;

    public class SetDecisionHandlerTests
    {
        private readonly IwsContext context;
        private readonly SetDecisionHandler handler;
        private readonly NotificationAssessment notificationAssessment;
        private readonly Guid notificationId = new Guid("5243D3E5-CA81-4A3E-B589-4D22D6676B28");

        public SetDecisionHandlerTests()
        {
            context = A.Fake<IwsContext>();
            handler = new SetDecisionHandler(context);
            var dbSetHelper = new DbContextHelper();
            notificationAssessment = new NotificationAssessment(notificationId);
            EntityHelper.SetEntityId(notificationAssessment, notificationId);

            var dbSet = dbSetHelper.GetAsyncEnabledDbSet(new NotificationAssessment[]
            {
                notificationAssessment
            });

            A.CallTo(() => context.NotificationAssessments).Returns(dbSet);
        }

        [Fact]
        public async Task SetDates()
        {
            var decisionDate = new DateTime(2015, 1, 3);
            var consentedFromDate = new DateTime(2015, 1, 4);
            var consentedToDate = new DateTime(2016, 1, 5);
            var decision = DecisionType.Consent;

            var request = new SetDecision
            {
                NotificationApplicationId = notificationId,
                DecisionMade = decisionDate,
                ConsentedFrom = consentedFromDate,
                ConsentedTo = consentedToDate,
                DecisionType = (int)decision
            };

            await handler.HandleAsync(request);

            Assert.True(notificationAssessment.ConsentedFrom == consentedFromDate
                        && notificationAssessment.ConsentedTo == consentedToDate && notificationAssessment.DecisionMade == decisionDate &&
                        notificationAssessment.DecisionType == (int)decision);
        }
    }
}
