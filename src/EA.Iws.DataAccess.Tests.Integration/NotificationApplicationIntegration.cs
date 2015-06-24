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

            var address = new Address("building", "address1", string.Empty, "town", string.Empty, string.Empty,
                "country");

            var business = new Business(string.Empty, String.Empty, String.Empty, string.Empty);

            var contact = new Contact(string.Empty, String.Empty, String.Empty, String.Empty);

            var producer = notification.AddProducer(business, address, contact);

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            var updateProducer = notification.Producers.Single(p => p.Id == producer.Id);
            var newAddress = new Address("new building", "address1", string.Empty, "town", string.Empty,
                string.Empty,
                "country");

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
            
            notification.SetPackagingInfo(new[] { PackagingInfo.CreatePackagingInfo(PackagingType.Bag) });

            await context.SaveChangesAsync();

            Assert.Equal(1, notification.PackagingInfos.Count());
        }

        [Fact]
        public async Task CanUpdateOperationCodes()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var codes = new List<OperationCode>
            {
                OperationCode.R1,
                OperationCode.R2
            };

            var newCodes = new List<OperationCode>
            {
                OperationCode.R3,
                OperationCode.R4
            };

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            notification.UpdateOperationCodes(codes);

            await context.SaveChangesAsync();

            notification.UpdateOperationCodes(newCodes);

            await context.SaveChangesAsync();

            Assert.Collection(notification.OperationInfos,
                item => Assert.Equal(notification.OperationInfos.ElementAt(0).OperationCode, OperationCode.R3),
                item => Assert.Equal(notification.OperationInfos.ElementAt(1).OperationCode, OperationCode.R4));

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CanAddWasteType()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            notification.AddWasteType(WasteType.CreateWoodWasteType("This waste type is of wood type. I am writing some description here."));

            await context.SaveChangesAsync();

            Assert.True(notification.HasWasteType);

            context.DeleteOnCommit(notification.WasteType);
            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CanAddTechnologyEmployed()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);

            context.NotificationApplications.Add(notification);
            
            notification.UpdateTechnologyEmployed(false, "text area contents");
            await context.SaveChangesAsync();

            Assert.True(notification.HasTechnologyEmployed);

            context.DeleteOnCommit(notification.TechnologyEmployed);
            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }
        
        [Fact]
        public async Task CanAddSpecialHandlingDetails()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
            UKCompetentAuthority.England, 0);

            notification.SetSpecialHandlingRequirements("keep upright");

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            Assert.True(notification.HasSpecialHandlingRequirements);
            Assert.Equal("keep upright", notification.SpecialHandlingDetails);

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }
    }
}