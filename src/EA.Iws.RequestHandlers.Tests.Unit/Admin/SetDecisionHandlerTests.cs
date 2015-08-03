namespace EA.Iws.RequestHandlers.Tests.Unit.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Admin;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
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
        private readonly NotificationDecision notificationDecision;
        private readonly Guid notificationId = new Guid("5243D3E5-CA81-4A3E-B589-4D22D6676B28");

        public SetDecisionHandlerTests()
        {
            context = A.Fake<IwsContext>();
            handler = new SetDecisionHandler(context);
            var dbSetHelper = new DbContextHelper();
            notificationDecision = new NotificationDecision(notificationId);
            EntityHelper.SetEntityId(notificationDecision, notificationId);

            var dbSet = dbSetHelper.GetAsyncEnabledDbSet(new List<NotificationDecision>
            {
                notificationDecision
            });

            var notification = new NotificationApplication(Guid.Empty, NotificationType.Recovery, UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(notification, notificationId);
            var notificationAssessments = dbSetHelper.GetAsyncEnabledDbSet(new List<NotificationApplication>
            {
                notification
            });

            A.CallTo(() => context.NotificationDecisions).Returns(dbSet);
            A.CallTo(() => context.NotificationApplications).Returns(notificationAssessments);
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
