namespace EA.Iws.DataAccess.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.RecoveryInfo;
    using Core.WasteType;
    using Domain;
    using Domain.Notification;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using TestHelpers.Helpers;
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

            var business = ObjectFactory.CreateEmptyProducerBusiness();

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

            notification.SetOperationCodes(codes);

            await context.SaveChangesAsync();

            notification.SetOperationCodes(newCodes);

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

            List<WasteComposition> wasteCompositions = new List<WasteComposition>
            {
                WasteComposition.CreateWasteComposition("First Constituent", 1, 100, ChemicalCompositionCategory.Metals),
                WasteComposition.CreateWasteComposition("Second Constituent", 2, 100, ChemicalCompositionCategory.Wood)
            };
            notification.AddWasteType(WasteType.CreateWoodWasteType("This waste type is of wood type. I am writing some description here.", wasteCompositions));

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

            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedDetails("text area contents"));
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

        [Fact]
        public async Task CanAddRecoveryInfo()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);

            context.NotificationApplications.Add(notification);

            notification.SetRecoveryInfoValues(RecoveryInfoUnits.Kilogram, 10, RecoveryInfoUnits.Tonne, 20.25M, RecoveryInfoUnits.Kilogram, 30);

            await context.SaveChangesAsync();

            Assert.Equal(true, notification.HasRecoveryInfo);
        }

        [Fact]
        public async Task CanAddRecoveryInfo_WithoutDisposal()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);

            context.NotificationApplications.Add(notification);

            notification.SetRecoveryInfoValues(RecoveryInfoUnits.Kilogram, 10, RecoveryInfoUnits.Tonne, -20, null, null);

            await context.SaveChangesAsync();

            Assert.Equal(true, notification.HasRecoveryInfo);
        }

        [Fact]
        public async Task CanAddRecoveryPercentageData()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
            UKCompetentAuthority.England, 0);

            notification.SetRecoveryPercentageData(56, "Some text");
            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();
 
            Assert.Equal(56, notification.PercentageRecoverable);
            Assert.Equal("Some text", notification.MethodOfDisposal);

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CanAddRecoveryPercentageDataProvidedByImporter()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
            UKCompetentAuthority.England, 0);

            notification.SetRecoveryPercentageDataProvidedByImporter();
            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            Assert.True(notification.IsProvidedByImporter);

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }
    }
}