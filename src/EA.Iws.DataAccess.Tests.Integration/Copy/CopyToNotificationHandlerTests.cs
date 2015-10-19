namespace EA.Iws.DataAccess.Tests.Integration.Copy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Domain.FinancialGuarantee;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Domain.TransportRoute;
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
        private const int SourceNumber = 99991;
        private const int DestinationNumber = 99992;
        private static readonly UKCompetentAuthority DestinationCompetentAuthority = UKCompetentAuthority.Scotland;
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
            context = new IwsContext(userContext, A.Fake<IEventDispatcher>());

            preRunNotifications = context.NotificationApplications.Select(na => na.Id).ToArray();

            source = NotificationApplicationFactory.CreateCompleted(new Guid("0ED9A007-3C35-48A3-B008-9D5623FA5AD9"),
                UserId,
                context.Countries.ToArray(),
                context.WasteCodes.ToArray(),
                SourceNumber);

            var transportRoute = TransportRouteFactory.CreateCompleted(
                new Guid("16CE4AE7-1FCF-4A04-84A3-9067DF24DEF6"), source.Id,
                context.EntryOrExitPoints.ToArray(),
                context.CompetentAuthorities.ToArray());

            destination = new NotificationApplication(UserId, DestinationNotificationType, DestinationCompetentAuthority, DestinationNumber);
            EntityHelper.SetEntityId(destination, new Guid("63581B29-EFB9-47F0-BCC3-E67382F4EAFA"));
            handler = new CopyToNotificationHandler(context, new NotificationToNotificationCopy(new WasteCodeCopy()), new TransportRouteToTransportRouteCopy());
            
            context.NotificationApplications.Add(source);
            context.TransportRoutes.Add(transportRoute);
            context.NotificationApplications.Add(destination);
            context.SaveChanges();

            var sourceAssessment = new NotificationAssessment(source.Id);
            var destinationAssessment = new NotificationAssessment(destination.Id);
            context.NotificationAssessments.Add(sourceAssessment);
            context.NotificationAssessments.Add(destinationAssessment);

            var sourceFinancialGuarantee = FinancialGuarantee.Create(source.Id);
            var destinationFinancialGuarantee = FinancialGuarantee.Create(destination.Id);
            context.FinancialGuarantees.Add(sourceFinancialGuarantee);
            context.FinancialGuarantees.Add(destinationFinancialGuarantee);
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

            Assert.Equal(sourceNotification.WasteCodes.OrderBy(wc => wc.CodeType).ThenBy(wc => wc.Id),
                copiedNotification.WasteCodes.OrderBy(wc => wc.CodeType).ThenBy(wc => wc.Id), new CodeComparer());
        }

        [Fact]
        public async Task Copy_CloneDoesNotCopyIntendedShipments()
        {
            var result = await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedNotification = GetCopied();
            var copiedShipmentInfo = context.ShipmentInfos.SingleOrDefault(si => si.NotificationId == copiedNotification.Id);

            var sourceNotification = GetSource();
            var sourceShipmentInfo = context.ShipmentInfos.SingleOrDefault(si => si.NotificationId == sourceNotification.Id);

            Assert.Null(copiedShipmentInfo);
            Assert.NotNull(sourceShipmentInfo);
        }

        [Fact]
        public async Task TransportRouteCopied()
        {
            var result = await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedTransport = GetCopiedTransportRoute();
            var sourceTransport = GetSourceTransportRoute();

            Assert.Equal(sourceTransport.StateOfExport.Country.Id, copiedTransport.StateOfExport.Country.Id);
            Assert.Equal(sourceTransport.StateOfExport.CompetentAuthority.Id, copiedTransport.StateOfExport.CompetentAuthority.Id);
            Assert.Equal(sourceTransport.StateOfExport.ExitPoint.Id, copiedTransport.StateOfExport.ExitPoint.Id);

            Assert.Equal(sourceTransport.StateOfImport.Country.Id, copiedTransport.StateOfImport.Country.Id);
            Assert.Equal(sourceTransport.StateOfImport.CompetentAuthority.Id, copiedTransport.StateOfImport.CompetentAuthority.Id);
            Assert.Equal(sourceTransport.StateOfImport.EntryPoint.Id, copiedTransport.StateOfImport.EntryPoint.Id);

            Assert.NotEqual(sourceTransport.StateOfImport.Id, copiedTransport.StateOfImport.Id);
            Assert.NotEqual(sourceTransport.StateOfExport.Id, copiedTransport.StateOfExport.Id);
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

        private TransportRoute GetSourceTransportRoute()
        {
            return context.TransportRoutes.Single(p => p.NotificationId == source.Id);
        }

        private TransportRoute GetCopiedTransportRoute()
        {
            return context.TransportRoutes.Single(n => n.NotificationId == destination.Id);
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

        private class CodeComparer : IEqualityComparer<WasteCodeInfo>
        {
            public bool Equals(WasteCodeInfo x, WasteCodeInfo y)
            {
                if (x == y)
                {
                    return true;
                }

                if (x.IsNotApplicable && y.IsNotApplicable)
                {
                    return true;
                }

                if (x.WasteCode != null && y.WasteCode != null)
                {
                    return x.WasteCode.Id == y.WasteCode.Id;
                }

                if (!string.IsNullOrWhiteSpace(x.CustomCode)
                    && !string.IsNullOrWhiteSpace(y.CustomCode))
                {
                    return x.CustomCode == y.CustomCode;
                }

                return false;
            }

            public int GetHashCode(WasteCodeInfo obj)
            {
                return base.GetHashCode();
            }
        }
    }
}