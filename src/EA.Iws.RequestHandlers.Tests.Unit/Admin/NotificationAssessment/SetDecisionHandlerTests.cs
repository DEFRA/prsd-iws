namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.Admin;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using RequestHandlers.Admin.NotificationAssessment;
    using Requests.Admin.NotificationAssessment;
    using TestHelpers.Helpers;
    using Xunit;

    public class SetDecisionHandlerTests
    {
        private readonly IwsContext context;
        private readonly SetDecisionHandler handler;
        private readonly NotificationDecision notificationDecision;
        private readonly Guid notificationId = new Guid("5243D3E5-CA81-4A3E-B589-4D22D6676B28");

        public SetDecisionHandlerTests()
        {
            context = new TestIwsContext();
            handler = new SetDecisionHandler(context);
            notificationDecision = new NotificationDecision(notificationId);
            EntityHelper.SetEntityId(notificationDecision, notificationId);

            var notification = new NotificationApplication(Guid.Empty, NotificationType.Recovery, UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(notification, notificationId);

            context.NotificationDecisions.Add(notificationDecision);
            context.NotificationApplications.Add(notification);
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

            Assert.True(notificationDecision.ConsentedFrom == consentedFromDate
                        && notificationDecision.ConsentedTo == consentedToDate && notificationDecision.DecisionMade == decisionDate &&
                        notificationDecision.DecisionType == (int)decision);
        }
    }
}
