namespace EA.Iws.DataAccess.Tests.Integration.Copy
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Domain.FinancialGuarantee;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Annexes;
    using Domain.NotificationApplication.Exporter;
    using Domain.NotificationApplication.Importer;
    using Domain.NotificationApplication.Shipment;
    using Domain.NotificationApplication.WasteRecovery;
    using Domain.NotificationAssessment;
    using Domain.TransportRoute;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Repositories;
    using RequestHandlers.Copy;
    using Requests.Copy;
    using Security;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;
    using NotificationApplicationFactory = TestHelpers.Helpers.NotificationApplicationFactory;

    [Trait("Category", "Integration")]
    public class CopyToNotificationHandlerTests : IDisposable
    {
        private static readonly Guid UserId = new Guid("7A354C6D-BA5D-49F7-8870-73B2E74E2677");
        private const int SourceNumber = 99991;
        private const int DestinationNumber = 99992;
        private static readonly CompetentAuthorityEnum DestinationCompetentAuthority = CompetentAuthorityEnum.England;
        private static readonly NotificationType DestinationNotificationType = NotificationType.Disposal;

        private readonly NotificationApplication source;
        private readonly NotificationApplication destination;
        private readonly IwsContext context;
        private readonly CopyToNotificationHandler handler;
        private readonly Guid[] preRunNotifications;

        public CopyToNotificationHandlerTests()
        {
            var applicationRepository = A.Fake<INotificationApplicationRepository>();
            SystemTime.Freeze(new DateTime(2015, 1, 1));
            context = new IwsContext(GetUserContext(), A.Fake<IEventDispatcher>());
            handler = new CopyToNotificationHandler(context,
                new NotificationToNotificationCopy(new WasteCodeCopy()),
                new ExporterToExporterCopy(),
                new TransportRouteToTransportRouteCopy(),
                new WasteRecoveryToWasteRecoveryCopy(),
                new ImporterToImporterCopy(),
                new NotificationApplicationRepository(context, new NotificationApplicationAuthorization(context, GetUserContext())),
                new FacilityCollectionCopy(),
                new CarrierCollectionCopy(),
                new ProducerCollectionCopy());

            preRunNotifications = context.NotificationApplications.Select(na => na.Id).ToArray();

            source = NotificationApplicationFactory.CreateCompleted(new Guid("0ED9A007-3C35-48A3-B008-9D5623FA5AD9"),
                UserId,
                context.Countries.ToArray(),
                context.WasteCodes.ToArray(),
                SourceNumber);

            destination = NotificationApplicationFactory.Create(UserId, DestinationNotificationType, DestinationCompetentAuthority, DestinationNumber);
            EntityHelper.SetEntityId(destination, new Guid("63581B29-EFB9-47F0-BCC3-E67382F4EAFA"));

            context.NotificationApplications.Add(source);
            context.NotificationApplications.Add(destination);

            // Calling source id and destination id is only safe after this call to save changes
            context.SaveChanges();

            AddNotificationLinkedEntities(source.Id, destination.Id);
            context.AnnexCollections.Add(new AnnexCollection(destination.Id));

            context.SaveChanges();

            A.CallTo(() => applicationRepository.GetById(A<Guid>.Ignored)).Returns(source);
        }

        private IUserContext GetUserContext()
        {
            var userContext = A.Fake<IUserContext>();
            A.CallTo(() => userContext.UserId).Returns(UserId);
            return userContext;
        }

        private void AddNotificationLinkedEntities(Guid sourceId, Guid destinationId)
        {
            AddShipmentInfo(sourceId);
            AddTransportRoute(sourceId);
            AddWasteRecovery(sourceId);
            AddExporter(sourceId);
            AddImporter(sourceId);

            AddNotificationAssessments(sourceId, destinationId);
            AddFinancialGuarantees(sourceId, destinationId);
        }

        private void AddShipmentInfo(Guid id)
        {
            context.ShipmentInfos.Add(new ShipmentInfo(id,
                new ShipmentPeriod(new DateTime(2015, 3, 3), new DateTime(2015, 5, 5), false), 25,
                new ShipmentQuantity(25, ShipmentQuantityUnits.CubicMetres)));
        }

        private void AddTransportRoute(Guid id)
        {
            var transportRoute = TransportRouteFactory.CreateCompleted(
                new Guid("16CE4AE7-1FCF-4A04-84A3-9067DF24DEF6"), id,
                context.EntryOrExitPoints.ToArray(),
                context.CompetentAuthorities.ToArray());
            context.TransportRoutes.Add(transportRoute);
        }

        private void AddWasteRecovery(Guid id)
        {
            var wasteRecovery = new WasteRecovery(id,
                new Percentage(100),
                new EstimatedValue(ValuePerWeightUnits.Kilogram, 10),
                new RecoveryCost(ValuePerWeightUnits.Kilogram, 5));

            context.WasteRecoveries.Add(wasteRecovery);
        }

        private void AddExporter(Guid id)
        {
            var exporter = new Exporter(id, TestableAddress.AddlestoneAddress, TestableBusiness.WasteSolutions, TestableContact.SinclairSimms);

            context.Exporters.Add(exporter);
        }

        private void AddImporter(Guid id)
        {
            var importer = new Importer(id, TestableAddress.AddlestoneAddress, TestableBusiness.WasteSolutions, TestableContact.SinclairSimms);

            context.Importers.Add(importer);
        }

        private void AddNotificationAssessments(Guid sourceId, Guid destinationId)
        {
            var sourceAssessment = new NotificationAssessment(sourceId);
            var destinationAssessment = new NotificationAssessment(destinationId);
            context.NotificationAssessments.Add(sourceAssessment);
            context.NotificationAssessments.Add(destinationAssessment);
        }

        private void AddFinancialGuarantees(Guid sourceId, Guid destinationId)
        {
            var sourceFinancialGuarantee = FinancialGuarantee.Create(sourceId);
            var destinationFinancialGuarantee = FinancialGuarantee.Create(destinationId);
            context.FinancialGuarantees.Add(sourceFinancialGuarantee);
            context.FinancialGuarantees.Add(destinationFinancialGuarantee);
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
            await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedNotification = GetCopied();

            Assert.Empty(context.NotificationApplications.Where(n => n.Id == destination.Id));
            Assert.Equal(DestinationNotificationType, copiedNotification.NotificationType);
            Assert.Equal(DestinationCompetentAuthority, copiedNotification.CompetentAuthority);
            Assert.Equal(destination.NotificationNumber, copiedNotification.NotificationNumber);
        }

        [Fact]
        public async Task CloneHasSameImporterDetails()
        {
            await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedImporter = GetCopiedImporter();
            var sourceImporter = GetSourceImporter();

            Assert.NotEqual(sourceImporter.Id, copiedImporter.Id);
            Assert.NotEqual(sourceImporter.NotificationId, copiedImporter.NotificationId);
            Assert.Equal(sourceImporter.Address, copiedImporter.Address, new AddressComparer());
            Assert.Equal(sourceImporter.Business, copiedImporter.Business, new BusinessComparer());
            Assert.Equal(sourceImporter.Contact, copiedImporter.Contact, new ContactComparer());
        }

        [Fact]
        public async Task CloneHasSameExporterDetails()
        {
            await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedExporter = GetCopiedExporter();
            var sourceExporter = GetSourceExporter();

            Assert.NotEqual(sourceExporter.Id, copiedExporter.Id);
            Assert.NotEqual(sourceExporter.NotificationId, copiedExporter.NotificationId);
            Assert.Equal(sourceExporter.Address, copiedExporter.Address, new AddressComparer());
            Assert.Equal(sourceExporter.Business, copiedExporter.Business, new BusinessComparer());
            Assert.Equal(sourceExporter.Contact, copiedExporter.Contact, new ContactComparer());
        }

        [Fact]
        public async Task CloneHasSameCodes()
        {
            await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedNotification = GetCopied();
            var sourceNotification = GetSource();

            Assert.Equal(sourceNotification.WasteCodes.OrderBy(wc => wc.CodeType).ThenBy(wc => wc.Id),
                copiedNotification.WasteCodes.OrderBy(wc => wc.CodeType).ThenBy(wc => wc.Id), new CodeComparer());
        }

        [Fact]
        public async Task Copy_CloneDoesNotCopyIntendedShipments()
        {
            await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

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
            await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

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
            await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

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
            await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedNotification = GetCopied();
            var sourceNotification = GetSource();

            Assert.Equal(sourceNotification.PackagingInfos.OrderBy(pi => pi.Id).Select(pi => pi.PackagingType),
                copiedNotification.PackagingInfos.OrderBy(pi => pi.Id).Select(pi => pi.PackagingType));
        }

        [Fact]
        public async Task AdditionalWasteInfosCopied()
        {
            await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedNotification = GetCopied();
            var sourceNotification = GetSource();

            Assert.Equal(sourceNotification.WasteType.WasteAdditionalInformation.Count(),
                copiedNotification.WasteType.WasteAdditionalInformation.Count());
            Assert.True(sourceNotification.WasteType.WasteAdditionalInformation.Any());
        }

        [Fact]
        public async Task WasteRecoveryCopied()
        {
            await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var copiedWasteRecovery = GetSourceWasteRecovery();
            var sourceWasteRecovery = GetCopiedWasteRecovery();

            Assert.NotEqual(sourceWasteRecovery.NotificationId, copiedWasteRecovery.NotificationId);
            Assert.Equal(sourceWasteRecovery, copiedWasteRecovery, new WasteRecoveryValuesComparer());
        }

        [Fact]
        public async Task WasteDisposalCopied()
        {
            var method = "Any Method";

            context.WasteDisposals.Add(new WasteDisposal(source.Id, method,
                new DisposalCost(ValuePerWeightUnits.Kilogram, 25)));

            context.SaveChanges();

            await handler.HandleAsync(new CopyToNotification(source.Id, destination.Id));

            var sourceWasteDisposal = await context.WasteDisposals.SingleAsync(w => w.NotificationId == source.Id);
            var copyId = GetCopied().Id;
            var copiedWasteDisposal = await context.WasteDisposals.SingleAsync(w => w.NotificationId == copyId);

            Assert.Equal(method, copiedWasteDisposal.Method);
            Assert.Equal(sourceWasteDisposal.Cost.Amount, copiedWasteDisposal.Cost.Amount);
            Assert.Equal(sourceWasteDisposal.Cost.Units, copiedWasteDisposal.Cost.Units);
        }

        private NotificationApplication GetCopied()
        {
            return context.NotificationApplications.Single(n => n.NotificationNumber == destination.NotificationNumber);
        }

        private NotificationApplication GetSource()
        {
            return context.NotificationApplications.Single(n => n.Id == source.Id);
        }

        private Guid GetCopiedId()
        {
            return context
                .NotificationApplications
                .Where(n => n.NotificationNumber == destination.NotificationNumber)
                .Select(n => n.Id).Single();
        }

        private TransportRoute GetSourceTransportRoute()
        {
            return context.TransportRoutes.Single(p => p.NotificationId == source.Id);
        }

        private TransportRoute GetCopiedTransportRoute()
        {
            var copiedId = GetCopiedId();
            return context.TransportRoutes.Single(n => n.NotificationId == copiedId);
        }

        private WasteRecovery GetSourceWasteRecovery()
        {
            return context.WasteRecoveries.Single(wr => wr.NotificationId == source.Id);
        }

        private WasteRecovery GetCopiedWasteRecovery()
        {
            var copiedId = GetCopiedId();
            return context.WasteRecoveries.Single(wr => wr.NotificationId == copiedId);
        }

        private Exporter GetSourceExporter()
        {
            return context.Exporters.Single(e => e.NotificationId == source.Id);
        }

        private Exporter GetCopiedExporter()
        {
            var copiedId = GetCopiedId();
            return context.Exporters.Single(e => e.NotificationId == copiedId);
        }

        private Importer GetSourceImporter()
        {
            return context.Importers.Single(e => e.NotificationId == source.Id);
        }

        private Importer GetCopiedImporter()
        {
            var copiedId = GetCopiedId();
            return context.Importers.Single(e => e.NotificationId == copiedId);
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

            SystemTime.Unfreeze();
        }

        private class CodeComparer : IEqualityComparer<WasteCodeInfo>
        {
            public bool Equals(WasteCodeInfo x, WasteCodeInfo y)
            {
                if (ReferenceEquals(x, y))
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

        private class WasteRecoveryValuesComparer : IEqualityComparer<WasteRecovery>
        {
            public bool Equals(WasteRecovery x, WasteRecovery y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                var isRecoveryCostEqual = x.RecoveryCost.Amount == y.RecoveryCost.Amount
                                          && x.RecoveryCost.Units == y.RecoveryCost.Units;

                var isEstimatedValueEqual = x.EstimatedValue.Amount == y.EstimatedValue.Amount
                          && x.EstimatedValue.Units == y.EstimatedValue.Units;

                var isPercentageRecoverableEqual = x.PercentageRecoverable.Value == y.PercentageRecoverable.Value;

                return isRecoveryCostEqual && isEstimatedValueEqual && isPercentageRecoverableEqual;
            }

            public int GetHashCode(WasteRecovery obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}