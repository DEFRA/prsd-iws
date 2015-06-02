namespace EA.Iws.DataAccess.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
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

        [Fact]
        public async Task CanModifyProducer()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var address = new Address(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                "United Kingdom");

            var business = new Business(string.Empty, String.Empty, String.Empty, string.Empty);

            var contact = new Contact(string.Empty, String.Empty, String.Empty, String.Empty);

            var producer = notification.AddProducer(business, address, contact);

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            var updateProducer = notification.Producers.Single(p => p.Id == producer.Id);
            var newAddress = new Address("new building", string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty,
                "United Kingdom");

            updateProducer.Address = newAddress;

            await context.SaveChangesAsync();

            var newBuildingName =
                await context.Database.SqlQuery<string>("SELECT Building FROM [Business].[Producer] WHERE Id = @id",
                    new SqlParameter("id", producer.Id)).SingleAsync();

            Assert.Equal("new building", newBuildingName);

            context.DeleteOnCommit(producer);
            context.DeleteOnCommit(notification);

            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CanAddPackagingInfo()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            context.NotificationApplications.Add(notification);
            
            notification.AddPackagingInfo(PackagingType.Bag);

            await context.SaveChangesAsync();

            Assert.True(notification.HasShipmentInfo);
        }
    }
}