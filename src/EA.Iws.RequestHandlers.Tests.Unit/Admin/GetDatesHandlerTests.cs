namespace EA.Iws.RequestHandlers.Tests.Unit.Admin
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using Helpers;
    using RequestHandlers.Admin;
    using Requests.Admin;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetDatesHandlerTests
    {
        private readonly IwsContext context;
        private readonly SetDatesHandler handler;
        private readonly NotificationAssessment notificationAssessment;
        private readonly Guid notificationId = new Guid("5243D3E5-CA81-4A3E-B589-4D22D6676B28");

        public GetDatesHandlerTests()
        {
            context = A.Fake<IwsContext>();
            handler = new SetDatesHandler(context);
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
                PaymentRecievedDate = receivedDate,
                TransmittedDate = transmittedDate
            };

            await handler.HandleAsync(request);

            Assert.True(notificationAssessment.AcknowledgedDate == acknowledgedDate
                        && notificationAssessment.CommencementDate == commencementDate && notificationAssessment.DecisionDate == decisionDate &&
                        notificationAssessment.CompleteDate == completeDate && notificationAssessment.PaymentRecievedDate == receivedDate &&
                        notificationAssessment.TransmittedDate == transmittedDate && notificationAssessment.NameOfOfficer == nameOfOfficer);
        }
    }
}
