namespace EA.Iws.DataAccess.Tests.Integration.Copy
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.Notification;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using RequestHandlers.Copy;
    using Requests.Copy;
    using TestHelpers.Helpers;
    using Xunit;

    [Trait("Category", "Integration")]
    public class CopyToNotificationHandlerTests : IDisposable
    {
        private static readonly Guid UserId = new Guid("7A354C6D-BA5D-49F7-8870-73B2E74E2677");
        private const int SourceNumber = 1;
        private const int DestinationNumber = 2;
        private static readonly UKCompetentAuthority SourceCompetentAuthority = UKCompetentAuthority.England;
        private static readonly UKCompetentAuthority DestinationCompetentAuthority = UKCompetentAuthority.Scotland;
        private static readonly NotificationType SourceNotificationType = NotificationType.Recovery;
        private static readonly NotificationType DestinationNotificationType = NotificationType.Disposal;

        private readonly NotificationApplication source;
        private readonly NotificationApplication destination;
        private readonly IwsContext context;
        private readonly CopyToNotificationHandler handler;
        private readonly Guid[] preRunNotifications;

        public CopyToNotificationHandlerTests()
        {
            var userContext = A.Fake<IUserContext>();
            A.CallTo(() => userContext.UserId).Returns(UserId);
            context = new IwsContext(userContext);

            preRunNotifications = context.NotificationApplications.Select(na => na.Id).ToArray();

            source = NotificationApplicationFactory.CreateCompleted(new Guid("0ED9A007-3C35-48A3-B008-9D5623FA5AD9"),
                UserId,
                context.Countries.ToArray(),
                context.EntryOrExitPoints.ToArray(),
                context.CompetentAuthorities.ToArray(),
                context.WasteCodes.ToArray(),
                SourceNumber);

            destination = new NotificationApplication(UserId, DestinationNotificationType, DestinationCompetentAuthority, DestinationNumber);
            EntityHelper.SetEntityId(destination, new Guid("63581B29-EFB9-47F0-BCC3-E67382F4EAFA"));
            handler = new CopyToNotificationHandler(context, userContext, new NotificationToNotificationCopy());

            context.NotificationApplications.Add(source);
            context.NotificationApplications.Add(destination);
            context.SaveChanges();
        }

        [Fact]
        public async Task DestinationDoesNotExist_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.HandleAsync(new CopyToNotification(source.Id, Guid.Empty)));

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.HandleAsync(new CopyToNotification(Guid.Empty, destination.Id)));
        }

        [Fact]
        public async Task CloneHasSourceNumberTypeAndAuthority()
        {
            var result = await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedNotification = GetCopied();

            Assert.Empty(context.NotificationApplications.Where(n => n.Id == destination.Id));
            Assert.Equal(DestinationNotificationType, copiedNotification.NotificationType);
            Assert.Equal(DestinationCompetentAuthority, copiedNotification.CompetentAuthority);
            Assert.Equal(destination.NotificationNumber, copiedNotification.NotificationNumber);
        }

        [Fact]
        public async Task CloneHasSameExporterAndImporterDetails()
        {
            var result = await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedNotification = GetCopied();
            var sourceNotification = GetSource();

            Assert.Equal(sourceNotification.Exporter.Business.Name, copiedNotification.Exporter.Business.Name);
            Assert.Equal(sourceNotification.Exporter.Business.RegistrationNumber, copiedNotification.Exporter.Business.RegistrationNumber);
            Assert.Equal(sourceNotification.Exporter.Contact.Email, copiedNotification.Exporter.Contact.Email);
            Assert.Equal(sourceNotification.Importer.Business.Name, copiedNotification.Importer.Business.Name);
            Assert.Equal(sourceNotification.Importer.Business.RegistrationNumber, copiedNotification.Importer.Business.RegistrationNumber);
            Assert.Equal(sourceNotification.Importer.Contact.Email, copiedNotification.Importer.Contact.Email);
            Assert.NotEqual(sourceNotification.Exporter.Id, copiedNotification.Exporter.Id);
            Assert.NotEqual(sourceNotification.Importer.Id, copiedNotification.Importer.Id);
        }

        [Fact]
        public async Task CloneHasSameCodes()
        {
            var result = await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedNotification = GetCopied();
            var sourceNotification = GetSource();

            Assert.Equal(sourceNotification.WasteCodes.Select(wc => wc.WasteCode).OrderBy(wc => wc.Id),
                copiedNotification.WasteCodes.Select(wc => wc.WasteCode).OrderBy(wc => wc.Id));
            Assert.Equal(sourceNotification.WasteCodes.Select(wci => wci.CustomCode).OrderBy(cc => cc),
                copiedNotification.WasteCodes.Select(wci => wci.CustomCode).OrderBy(cc => cc));
        }

        [Fact]
        public async Task Copy_CloneDoesNotCopyIntendedShipments()
        {
            var result = await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedNotification = GetCopied();
            var sourceNotification = GetSource();

            Assert.Null(copiedNotification.ShipmentInfo);
            Assert.NotNull(sourceNotification.ShipmentInfo);
        }

        [Fact]
        public async Task TransportRouteCopied()
        {
            var result = await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedNotification = GetCopied();
            var sourceNotification = GetSource();

            Assert.Equal(sourceNotification.StateOfExport.Country.Id, copiedNotification.StateOfExport.Country.Id);
            Assert.Equal(sourceNotification.StateOfExport.CompetentAuthority.Id, copiedNotification.StateOfExport.CompetentAuthority.Id);
            Assert.Equal(sourceNotification.StateOfExport.ExitPoint.Id, copiedNotification.StateOfExport.ExitPoint.Id);

            Assert.Equal(sourceNotification.StateOfImport.Country.Id, copiedNotification.StateOfImport.Country.Id);
            Assert.Equal(sourceNotification.StateOfImport.CompetentAuthority.Id, copiedNotification.StateOfImport.CompetentAuthority.Id);
            Assert.Equal(sourceNotification.StateOfImport.EntryPoint.Id, copiedNotification.StateOfImport.EntryPoint.Id);

            Assert.NotEqual(sourceNotification.StateOfImport.Id, copiedNotification.StateOfImport.Id);
            Assert.NotEqual(sourceNotification.StateOfExport.Id, copiedNotification.StateOfExport.Id);
        }

        [Fact]
        public async Task OperationDetailsCopied()
        {
            var result = await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedNotification = GetCopied();
            var sourceNotification = GetSource();

            Assert.Equal(sourceNotification.OperationInfos.Select(oi => oi.OperationCode).OrderBy(oc => oc.Value),
                copiedNotification.OperationInfos.Select(oi => oi.OperationCode).OrderBy(oc => oc.Value));
            Assert.Equal(sourceNotification.TechnologyEmployed.Details, copiedNotification.TechnologyEmployed.Details);
            Assert.Equal(sourceNotification.ReasonForExport, copiedNotification.ReasonForExport);
        }

        [Fact]
        public async Task PackagingTypesCopied()
        {
            var result = await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedNotification = GetCopied();
            var sourceNotification = GetSource();

            Assert.Equal(sourceNotification.PackagingInfos.OrderBy(pi => pi.Id).Select(pi => pi.PackagingType),
                copiedNotification.PackagingInfos.OrderBy(pi => pi.Id).Select(pi => pi.PackagingType));
        }

        [Fact]
        public async Task ProducersCopied()
        {
            var result = await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedNotification = GetCopied();
            var sourceNotification = GetSource();

            Assert.Equal(sourceNotification.Producers.Count(), 
                copiedNotification.Producers.Count());
            Assert.Equal(sourceNotification.Producers.Select(p => p.Business.Name).OrderBy(s => s),
                copiedNotification.Producers.Select(p => p.Business.Name).OrderBy(s => s));
        }

        [Fact]
        public async Task CarriersCopied()
        {
            var result = await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedNotification = GetCopied();
            var sourceNotification = GetSource();

            Assert.Equal(sourceNotification.Carriers.Count(),
                copiedNotification.Carriers.Count());
            Assert.Equal(sourceNotification.Carriers.Select(c => c.Business.Name).OrderBy(s => s),
                copiedNotification.Carriers.Select(c => c.Business.Name).OrderBy(s => s));
        }

        [Fact]
        public async Task AdditionalWasteInfosCopied()
        {
            var result = await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedNotification = GetCopied();
            var sourceNotification = GetSource();

            Assert.Equal(sourceNotification.WasteType.WasteAdditionalInformation.Count(), 
                copiedNotification.WasteType.WasteAdditionalInformation.Count());
            Assert.True(sourceNotification.WasteType.WasteAdditionalInformation.Any());
        }

        private NotificationApplication GetCopied()
        {
            return context.NotificationApplications.Single(n => n.NotificationNumber == destination.NotificationNumber);
        }

        private NotificationApplication GetSource()
        {
            return context.NotificationApplications.Single(n => n.Id == source.Id);
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