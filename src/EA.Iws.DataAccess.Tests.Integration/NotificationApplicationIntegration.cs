namespace EA.Iws.DataAccess.Tests.Integration
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using Domain.Notification;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using Xunit;

    public class NotificationApplicationIntegration
    {
        private readonly IwsContext context;

        public NotificationApplicationIntegration()
        {
            var userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(Guid.NewGuid());

            context = new IwsContext(userContext);
        }

        private Producer CreateEmptyProducer()
        {
            var address = new Address(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                "United Kingdom");

            var business = new Business(string.Empty, String.Empty, String.Empty, string.Empty);

            var contact = new Contact(string.Empty, String.Empty, String.Empty, String.Empty);

            return new Producer(business, address, contact);
        }

        [Fact]
        public async Task CanModifyProducer()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
            var producer = CreateEmptyProducer();

            notification.AddProducer(producer);

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            var updateProducer = notification.Producers.Single(p => p.Id == producer.Id);
            var newAddress = new Address("new building", string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty,
                "United Kingdom");

            // TODO - no way to update Producer yet so using reflection to change values
            typeof(Producer).GetProperty("Address").SetValue(updateProducer, newAddress);

            await context.SaveChangesAsync();
        }
    }
}