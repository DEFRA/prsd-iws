namespace EA.Iws.RequestHandlers.Tests.Unit.Admin
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using RequestHandlers.Admin;
    using Requests.Admin;
    using TestHelpers.Helpers;
    using Xunit;

    public class SetDatesHandlerTests
    {
        private readonly IwsContext context;
        private readonly SetDatesHandler handler;
        private readonly NotificationDates notificationDates;
        private readonly Guid notificationId = new Guid("5243D3E5-CA81-4A3E-B589-4D22D6676B28");

        public SetDatesHandlerTests()
        {
            context = new TestIwsContext();
            handler = new SetDatesHandler(context);
            notificationDates = new NotificationDates(notificationId);
            EntityHelper.SetEntityId(notificationDates, notificationId);

            var notification = new NotificationApplication(Guid.Empty, NotificationType.Recovery, UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(notification, notificationId);

            context.NotificationDates.Add(notificationDates);
            context.NotificationApplications.Add(notification);
        }

        [Fact]
        public async Task SetDates()
        {
            var acknowledgedDate = new DateTime(2015, 1, 1);
            var commencementDate = new DateTime(2015, 1, 2);
            var decisionDate = new DateTime(2015, 1, 3);
            var completeDate = new DateTime(2015, 1, 4);
            var receivedDate = new DateTime(2015, 1, 5);
            var transmittedDate = new DateTime(2015, 1, 5);
            var nameOfOfficer = "name";

            var request = new SetDates
            {
                NotificationApplicationId = notificationId,
                NameOfOfficer = nameOfOfficer,
                AcknowledgedDate = acknowledgedDate,
                CommencementDate = commencementDate,
                DecisionDate = decisionDate,
                CompleteDate = completeDate,
                PaymentReceivedDate = receivedDate,
                TransmittedDate = transmittedDate
            };

            await handler.HandleAsync(request);

            Assert.True(notificationDates.AcknowledgedDate == acknowledgedDate
                        && notificationDates.CommencementDate == commencementDate && notificationDates.DecisionDate == decisionDate &&
                        notificationDates.CompleteDate == completeDate && notificationDates.PaymentReceivedDate == receivedDate &&
                        notificationDates.TransmittedDate == transmittedDate && notificationDates.NameOfOfficer == nameOfOfficer);
        }
    }
}
