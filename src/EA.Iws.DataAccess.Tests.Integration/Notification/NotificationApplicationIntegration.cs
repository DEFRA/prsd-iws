﻿namespace EA.Iws.DataAccess.Tests.Integration.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.OperationCodes;
    using Core.PackagingType;
    using Core.Shared;
    using Core.WasteType;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using TestHelpers.Helpers;
    using Xunit;
    using NotificationApplicationFactory = TestHelpers.Helpers.NotificationApplicationFactory;

    [Trait("Category", "Integration")]
    public class NotificationApplicationIntegration : IDisposable
    {
        private readonly IwsContext context;
        private readonly Guid[] preRunNotifications;

        public NotificationApplicationIntegration()
        {
            var userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(Guid.NewGuid());

            context = new IwsContext(userContext, A.Fake<IEventDispatcher>());

            preRunNotifications = context.NotificationApplications.Select(na => na.Id).ToArray();
        }

        [Fact]
        public async Task CanAddMultipleProducers()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyProducerBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            var producerCollection = new ProducerCollection(notification.Id);

            for (int i = 0; i < 5; i++)
            {
                producerCollection.AddProducer(business, address, contact);
            }

            context.Producers.Add(producerCollection);
            await context.SaveChangesAsync();

            var dbProducerCollection = context.Producers.Single(p => p.NotificationId == notification.Id);

            Assert.True(producerCollection.Producers.Count() == dbProducerCollection.Producers.Count());
        }

        [Fact]
        public async Task CanModifyProducer()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var address = new Address("address1", string.Empty, "town", string.Empty, string.Empty, "country");
            var business = ObjectFactory.CreateEmptyProducerBusiness();
            var contact = new Contact(string.Empty, String.Empty, String.Empty, String.Empty);

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            var producerCollection = new ProducerCollection(notification.Id);

            var producer = producerCollection.AddProducer(business, address, contact);

            context.Producers.Add(producerCollection);
            await context.SaveChangesAsync();

            var updateProducer = producerCollection.GetProducer(producer.Id);
            var newAddress = new Address("address1", string.Empty, "town", string.Empty, string.Empty, "country");

            updateProducer.Address = newAddress;

            await context.SaveChangesAsync();

            var newAddress1 =
                await context.Database.SqlQuery<string>("SELECT [Address1] FROM [Notification].[Producer] WHERE Id = @id",
                    new SqlParameter("id", producer.Id)).SingleAsync();

            Assert.Equal("address1", newAddress1);
        }

        [Fact]
        public async Task CanAddPackagingInfo()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            context.NotificationApplications.Add(notification);

            notification.SetPackagingInfo(new[] { PackagingInfo.CreatePackagingInfo(PackagingType.Bag) });

            await context.SaveChangesAsync();

            Assert.Single(notification.PackagingInfos);
        }

        [Fact]
        public async Task CanUpdateOperationCodes()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
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
                item => Assert.Equal(OperationCode.R3, notification.OperationInfos.ElementAt(0).OperationCode),
                item => Assert.Equal(OperationCode.R4, notification.OperationInfos.ElementAt(1).OperationCode));
        }

        [Fact]
        public async Task CanAddWasteType()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            List<WasteAdditionalInformation> wasteCompositions = new List<WasteAdditionalInformation>
            {
                WasteAdditionalInformation.CreateWasteAdditionalInformation("boulder", 5, 10, WasteInformationType.Energy),
                WasteAdditionalInformation.CreateWasteAdditionalInformation("notes", 6, 9, WasteInformationType.AshContent)
            };
            notification.SetWasteType(WasteType.CreateWoodWasteType("This waste type is of wood type. I am writing some description here.", wasteCompositions));

            await context.SaveChangesAsync();

            Assert.True(notification.HasWasteType);
        }

        [Fact]
        public async Task CanAddSpecialHandlingDetails()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
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
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyProducerBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            var producerCollection = new ProducerCollection(notification.Id);

            var producer = producerCollection.AddProducer(business, address, contact);

            Assert.True(producerCollection.Producers.Any());

            producerCollection.RemoveProducer(producer.Id);
            await context.SaveChangesAsync();

            Assert.False(producerCollection.Producers.Any());
        }

        [Fact]
        public async Task CanNotRemoveSiteOfExportProducerForMoreThanOneProducers()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyProducerBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            var producerCollection = new ProducerCollection(notification.Id);

            producerCollection.AddProducer(business, address, contact);
            var anotherProducer = producerCollection.AddProducer(business, address, contact);

            Assert.True(producerCollection.Producers.Count() == 2);

            producerCollection.SetProducerAsSiteOfExport(anotherProducer.Id);
            await context.SaveChangesAsync();

            Action removeProducer = () => producerCollection.RemoveProducer(anotherProducer.Id);
            Assert.Throws<InvalidOperationException>(removeProducer);
        }

        [Fact]
        public async Task CanRemoveProducerOtherThanSiteOfExporter()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyProducerBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            var producerCollection = new ProducerCollection(notification.Id);

            var producer = producerCollection.AddProducer(business, address, contact);
            var anotherProducer = producerCollection.AddProducer(business, address, contact);
            context.NotificationApplications.Add(notification);
            context.Producers.Add(producerCollection);
            await context.SaveChangesAsync();

            Assert.True(producerCollection.Producers.Count() == 2);

            producerCollection.SetProducerAsSiteOfExport(producer.Id);
            await context.SaveChangesAsync();

            producerCollection.RemoveProducer(anotherProducer.Id);
            await context.SaveChangesAsync();

            Assert.True(producerCollection.Producers.Count() == 1);
        }

        [Fact]
        public async Task CanAddMultipleFacilities()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery,
                                UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            var facilityCollection = new FacilityCollection(notification.Id);
            context.Facilities.Add(facilityCollection);
            await context.SaveChangesAsync();

            for (int i = 0; i < 5; i++)
            {
                facilityCollection.AddFacility(business, address, contact);
            }

            await context.SaveChangesAsync();

            Assert.True(facilityCollection.Facilities.Count() == 5);
        }

        [Fact]
        public async Task CanRemoveFacility()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            var facilityCollection = new FacilityCollection(notification.Id);
            context.Facilities.Add(facilityCollection);
            await context.SaveChangesAsync();

            var facility = facilityCollection.AddFacility(business, address, contact);
            await context.SaveChangesAsync();

            Assert.True(facilityCollection.Facilities.Any());

            facilityCollection.RemoveFacility(facility.Id);
            await context.SaveChangesAsync();

            Assert.False(facilityCollection.Facilities.Any());
        }

        [Fact]
        public async Task CanNotRemoveActualSiteOfTreatmentFacilityForMoreThanOneFacilities()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            var facilityCollection = new FacilityCollection(notification.Id);
            context.Facilities.Add(facilityCollection);
            await context.SaveChangesAsync();

            facilityCollection.AddFacility(business, address, contact);
            var anotherFacility = facilityCollection.AddFacility(business, address, contact);

            await context.SaveChangesAsync();

            Assert.True(facilityCollection.Facilities.Count() == 2);

            facilityCollection.SetFacilityAsSiteOfTreatment(anotherFacility.Id);
            await context.SaveChangesAsync();

            Action removeFacility = () => facilityCollection.RemoveFacility(anotherFacility.Id);
            Assert.Throws<InvalidOperationException>(removeFacility);
        }

        [Fact]
        public async Task CanRemoveFacilityOtherThanActualSiteOfTreatment()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            var facilityCollection = new FacilityCollection(notification.Id);
            context.Facilities.Add(facilityCollection);
            await context.SaveChangesAsync();

            var facility = facilityCollection.AddFacility(business, address, contact);
            var anotherFacility = facilityCollection.AddFacility(business, address, contact);

            await context.SaveChangesAsync();

            Assert.True(facilityCollection.Facilities.Count() == 2);

            facilityCollection.SetFacilityAsSiteOfTreatment(facility.Id);
            await context.SaveChangesAsync();

            facilityCollection.RemoveFacility(anotherFacility.Id);
            await context.SaveChangesAsync();

            Assert.True(facilityCollection.Facilities.Count() == 1);
        }

        [Fact]
        public async Task CanAddMultipleCarriers()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyProducerBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            var carrierCollection = new CarrierCollection(notification.Id);
            context.Carriers.Add(carrierCollection);
            await context.SaveChangesAsync();

            for (int i = 0; i < 5; i++)
            {
                carrierCollection.AddCarrier(business, address, contact);
            }

            await context.SaveChangesAsync();

            Assert.Equal(5, carrierCollection.Carriers.Count());
        }

        [Fact]
        public async Task CanRemoveCarrier()
        {
            var notification = NotificationApplicationFactory.Create(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);

            var address = ObjectFactory.CreateDefaultAddress();
            var business = ObjectFactory.CreateEmptyProducerBusiness();
            var contact = ObjectFactory.CreateEmptyContact();

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            var carrierCollection = new CarrierCollection(notification.Id);
            context.Carriers.Add(carrierCollection);
            await context.SaveChangesAsync();

            var carrier = carrierCollection.AddCarrier(business, address, contact);
            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            Assert.True(carrierCollection.Carriers.Any());

            carrierCollection.RemoveCarrier(carrier.Id);
            await context.SaveChangesAsync();

            Assert.False(carrierCollection.Carriers.Any());
        }

        public void Dispose()
        {
            var createdNotifications =
                context.NotificationApplications.Where(n => !preRunNotifications.Contains(n.Id))
                    .Select(n => n.Id)
                    .ToArray();

            foreach (var createdNotification in createdNotifications)
            {
                DatabaseDataDeleter.DeleteDataForNotification(createdNotification, context);
            }

            context.Dispose();
        }
    }
}