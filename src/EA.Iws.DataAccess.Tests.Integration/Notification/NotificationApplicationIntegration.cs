namespace EA.Iws.DataAccess.Tests.Integration.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.WasteType;
    using Domain;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using TestHelpers.Helpers;
    using Xunit;
    using NotificationType = Domain.NotificationApplication.NotificationType;

    [Trait("Category", "Integration")]
    public class NotificationApplicationIntegration
    {
        private readonly IwsContext context;

        public NotificationApplicationIntegration()
        {
            var userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(Guid.NewGuid());

            context = new IwsContext(userContext, A.Fake<IEventDispatcher>());
        }

        [Fact]
        public async Task CanAddMultipleProducers()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyProducerBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            for (int i = 0; i < 5; i++)
            {
                notification.AddProducer(business, address, contact);
            }

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();
            Assert.True(notification.Producers.Count() == 5);

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CanModifyProducer()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var address = new Address("address1", string.Empty, "town", string.Empty, string.Empty, "country");

            var business = ObjectFactory.CreateEmptyProducerBusiness();

            var contact = new Contact(string.Empty, String.Empty, String.Empty, String.Empty);

            var producer = notification.AddProducer(business, address, contact);

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            var updateProducer = notification.Producers.Single(p => p.Id == producer.Id);
            var newAddress = new Address("address1", string.Empty, "town", string.Empty, string.Empty, "country");

            updateProducer.Address = newAddress;

            await context.SaveChangesAsync();

            var newAddress1 =
                await context.Database.SqlQuery<string>("SELECT [Address1] FROM [Notification].[Producer] WHERE Id = @id",
                    new SqlParameter("id", producer.Id)).SingleAsync();

            Assert.Equal("address1", newAddress1);

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
            notification.SetWasteType(WasteType.CreateWoodWasteType("This waste type is of wood type. I am writing some description here.", wasteCompositions));

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

            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails("text area contents", "details"));
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
        public async Task CanRemoveProducer()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyProducerBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            var producer = notification.AddProducer(business, address, contact);
            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            Assert.True(notification.Producers.Any());

            notification.RemoveProducer(producer.Id);
            await context.SaveChangesAsync();

            Assert.False(notification.Producers.Any());

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CanNotRemoveSiteOfExportProducerForMoreThanOneProducers()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyProducerBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            notification.AddProducer(business, address, contact);
            var anotherProducer = notification.AddProducer(business, address, contact);
            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            Assert.True(notification.Producers.Count() == 2);

            notification.SetProducerAsSiteOfExport(anotherProducer.Id);
            await context.SaveChangesAsync();

            Action removeProducer = () => notification.RemoveProducer(anotherProducer.Id);
            Assert.Throws<InvalidOperationException>(removeProducer);

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CanRemoveProducerOtherThanSiteOfExporter()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyProducerBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            var producer = notification.AddProducer(business, address, contact);
            var anotherProducer = notification.AddProducer(business, address, contact);
            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            Assert.True(notification.Producers.Count() == 2);

            notification.SetProducerAsSiteOfExport(producer.Id);
            await context.SaveChangesAsync();

            notification.RemoveProducer(anotherProducer.Id);
            await context.SaveChangesAsync();

            Assert.True(notification.Producers.Count() == 1);

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CanAddMultipleFacilities()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                                UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            for (int i = 0; i < 5; i++)
            {
                notification.AddFacility(business, address, contact);
            }

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();
            Assert.True(notification.Facilities.Count() == 5);

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CanRemoveFacility()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            var facility = notification.AddFacility(business, address, contact);
            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            Assert.True(notification.Facilities.Any());

            notification.RemoveFacility(facility.Id);
            await context.SaveChangesAsync();

            Assert.False(notification.Facilities.Any());

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CanNotRemoveActualSiteOfTreatmentFacilityForMoreThanOneFacilities()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            notification.AddFacility(business, address, contact);
            var anotherFacility = notification.AddFacility(business, address, contact);
            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            Assert.True(notification.Facilities.Count() == 2);

            notification.SetFacilityAsSiteOfTreatment(anotherFacility.Id);
            await context.SaveChangesAsync();

            Action removeFacility = () => notification.RemoveFacility(anotherFacility.Id);
            Assert.Throws<InvalidOperationException>(removeFacility);

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CanRemoveFacilityOtherThanActualSiteOfTreatment()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            var facility = notification.AddFacility(business, address, contact);
            var anotherFacility = notification.AddFacility(business, address, contact);
            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            Assert.True(notification.Facilities.Count() == 2);

            notification.SetFacilityAsSiteOfTreatment(facility.Id);
            await context.SaveChangesAsync();

            notification.RemoveFacility(anotherFacility.Id);
            await context.SaveChangesAsync();

            Assert.True(notification.Facilities.Count() == 1);

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CanAddMultipleCarriers()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyProducerBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            for (int i = 0; i < 5; i++)
            {
                notification.AddCarrier(business, address, contact);
            }

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();
            Assert.Equal(notification.Carriers.Count(), 5);

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CanRemoveCarrier()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyProducerBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            var carrier = notification.AddCarrier(business, address, contact);
            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            Assert.True(notification.Carriers.Any());

            notification.RemoveCarrier(carrier.Id);
            await context.SaveChangesAsync();

            Assert.False(notification.Carriers.Any());

            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }
    }
}