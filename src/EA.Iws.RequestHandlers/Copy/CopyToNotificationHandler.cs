﻿namespace EA.Iws.RequestHandlers.Copy
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification;
    using DataAccess;
    using Domain;
    using Domain.FinancialGuarantee;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Annexes;
    using Domain.NotificationAssessment;
    using Domain.TransportRoute;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.Copy;

    internal class CopyToNotificationHandler : IRequestHandler<CopyToNotification, Guid>
    {
        private readonly IwsContext context;
        private readonly NotificationToNotificationCopy copier;
        private readonly ExporterToExporterCopy exporterCopier;
        private readonly TransportRouteToTransportRouteCopy transportRouteCopier;
        private readonly WasteRecoveryToWasteRecoveryCopy wasteRecoveryCopier;
        private readonly ImporterToImporterCopy importerCopier;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly IIntraCountryExportAllowedRepository intraCountryExportAllowedRepository;
        private readonly FacilityCollectionCopy facilityCopier;
        private readonly CarrierCollectionCopy carrierCopier;
        private readonly ProducerCollectionCopy producerCopier;
        private readonly MeansOfTransportToMeansOfTransportCopy meansOfTransportCopier;

        public CopyToNotificationHandler(IwsContext context,
            NotificationToNotificationCopy copier,
            ExporterToExporterCopy exporterCopier,
            TransportRouteToTransportRouteCopy transportRouteCopier,
            WasteRecoveryToWasteRecoveryCopy wasteRecoveryCopier,
            ImporterToImporterCopy importerCopier,
            INotificationApplicationRepository notificationApplicationRepository,
            IIntraCountryExportAllowedRepository intraCountryExportAllowedRepository,
            FacilityCollectionCopy facilityCopier,
            CarrierCollectionCopy carrierCopier,
            ProducerCollectionCopy producerCopier,
            MeansOfTransportToMeansOfTransportCopy meansOfTransportCopier)
        {
            this.context = context;
            this.copier = copier;
            this.exporterCopier = exporterCopier;
            this.transportRouteCopier = transportRouteCopier;
            this.wasteRecoveryCopier = wasteRecoveryCopier;
            this.importerCopier = importerCopier;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.intraCountryExportAllowedRepository = intraCountryExportAllowedRepository;
            this.facilityCopier = facilityCopier;
            this.carrierCopier = carrierCopier;
            this.producerCopier = producerCopier;
            this.meansOfTransportCopier = meansOfTransportCopier;
        }

        public async Task<Guid> HandleAsync(CopyToNotification message)
        {
            var id = Guid.Empty;
            using (var transaction = BeginTransaction())
            {
                try
                {
                    var clone = await CloneNotification(message.SourceId, message.DestinationId);

                    if (GetChangedEntities(EntityState.Deleted).Any())
                    {
                        throw new InvalidOperationException(string.Format("The copy operation from notification {0} to {1} failed because {2}",
                            message.SourceId,
                            message.DestinationId,
                            "the operation would have deleted child entities."));
                    }

                    await context.SaveChangesAsync();

                    if (await HasDuplicateNotifications())
                    {
                        transaction.Rollback();
                    }
                    else
                    {
                        transaction.Commit();
                        id = clone.Id;
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return id;
        }

        /// <summary>
        /// Create a clone of a notification excluding shipment info.
        /// </summary>
        /// <param name="sourceId">The notification id to copy from</param>
        /// <param name="destinationId">The notification id to copy to</param>
        /// <returns>the clone of the notification</returns>
        private async Task<NotificationApplication> CloneNotification(Guid sourceId, Guid destinationId)
        {
            var destination = await context.GetNotificationApplication(destinationId);
            var destinationAssessment = await context.NotificationAssessments.SingleAsync(p => p.NotificationApplicationId == destinationId);
            var destinationFinancialGuarantee = await context.FinancialGuarantees.SingleAsync(fg => fg.NotificationId == destinationId);
            var destinationAnnexCollection = await context.AnnexCollections.SingleAsync(p => p.NotificationId == destinationId);
            var destinationFacilities = await context.Facilities.SingleAsync(f => f.NotificationId == destinationId);
            var destinationCarriers = await context.Carriers.SingleAsync(c => c.NotificationId == destinationId);
            var destinationProducers = await context.Producers.SingleAsync(p => p.NotificationId == destinationId);

            var clone = await GetCopyOfSourceNotification(sourceId);
            var clonedAssessment = await GetCopyOfNotificationAssessment(destinationId);

            copier.CopyNotificationProperties(clone, destination);

            context.NotificationApplications.Add(clone);

            // Remove the destination.
            context.DeleteOnCommit(destinationAssessment);
            context.DeleteOnCommit(destinationFinancialGuarantee);
            context.DeleteOnCommit(destinationAnnexCollection);
            context.DeleteOnCommit(destinationFacilities);
            context.DeleteOnCommit(destinationCarriers);
            context.DeleteOnCommit(destinationProducers);
            await context.SaveChangesAsync();

            context.DeleteOnCommit(destination);
            await context.SaveChangesAsync();

            // Set child objects which have lookup properties. These can't be copied by the
            // main copy process due to detaching the change tracker.
            var source = await context.GetNotificationApplication(sourceId);
            copier.CopyLookupEntities(source, clone);

            // Update foreign key on assessment object
            SetNotificationApplicationIdOnAssessment(clonedAssessment, clone.Id);
            context.NotificationAssessments.Add(clonedAssessment);

            // Add financial guarantee
            context.FinancialGuarantees.Add(new FinancialGuaranteeCollection(clone.Id));

            // Transport route
            await CloneTransportRoute(sourceId, clone.Id, destination.CompetentAuthority);
            await wasteRecoveryCopier.CopyAsync(context, sourceId, clone.Id);
            await exporterCopier.CopyAsync(context, sourceId, clone.Id);
            await importerCopier.CopyAsync(context, sourceId, clone.Id);
            await facilityCopier.CopyAsync(context, sourceId, clone.Id);
            await carrierCopier.CopyAsync(context, sourceId, clone.Id);
            await producerCopier.CopyAsync(context, sourceId, clone.Id);
            await meansOfTransportCopier.CopyAsync(context, sourceId, clone.Id);
            await CloneTechnologyEmployed(sourceId, clone.Id);

            context.AnnexCollections.Add(new AnnexCollection(clone.Id));

            return clone;
        }

        /// <summary>
        /// Create a detached copy of the source notification.
        /// This excludes intended shipments intentionally.
        /// This does not copy child objects where there is a navigation property on that child object since
        /// the change tracker is detached.
        /// </summary>
        /// <param name="id">The id of the notification to copy.</param>
        /// <returns>A partial copy of a notification.</returns>
        private async Task<NotificationApplication> GetCopyOfSourceNotification(Guid id)
        {
            // We have to force eager loading by including all navigation properties.
            //TODO: Filter by status
            var clone = await context.Set<NotificationApplication>()
                .AsNoTracking()
                .Include("OperationInfosCollection")
                .Include("PackagingInfosCollection")
                .Include("WasteType.WasteCompositionCollection")
                .Include("WasteType.WasteAdditionalInformationCollection")
                .Include("PhysicalCharacteristicsCollection")
                .SingleAsync(n => n.Id == id);

            return clone;
        }

        /// <summary>
        /// Create a detached copy of a notification assessment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<NotificationAssessment> GetCopyOfNotificationAssessment(Guid id)
        {
            return await context.NotificationAssessments
                .AsNoTracking()
                .Include(na => na.Dates)
                .SingleAsync(p => p.NotificationApplicationId == id);
        }

        private static void SetNotificationApplicationIdOnAssessment(NotificationAssessment assessment, Guid notificationId)
        {
            typeof(NotificationAssessment).GetProperty("NotificationApplicationId").SetValue(assessment, notificationId, null);
        }

        /// <summary>
        /// Checks if there are any duplicate notifications in the context.
        /// </summary>
        /// <returns>true if there are any duplicate notifications; otherwise false.</returns>
        private async Task<bool> HasDuplicateNotifications()
        {
            var notificationCount = await context.NotificationApplications
                .Select(n => n.NotificationNumber)
                .GroupBy(n => n).CountAsync();

            return (notificationCount != context.NotificationApplications.Count());
        }

        protected virtual IEnumerable<DbEntityEntry> GetChangedEntities(params EntityState[] states)
        {
            Guard.ArgumentNotNull(() => states, states);

            return context.ChangeTracker.Entries()
                .Where(e => states.Contains(e.State));
        }

        protected virtual DbContextTransaction BeginTransaction()
        {
            return context.Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        private async Task<TransportRoute> CloneTransportRoute(Guid sourceNotificationId, Guid destinationNotificationId, UKCompetentAuthority destinationCompetentAuthority)
        {
            var transportRoute = await context.TransportRoutes.SingleOrDefaultAsync(p => p.NotificationId == sourceNotificationId);

            var destinationTransportRoute = new TransportRoute(destinationNotificationId);

            if (transportRoute != null)
            {
                var sourceCompetentAuthority = (await notificationApplicationRepository.GetById(sourceNotificationId)).CompetentAuthority;
                var intraCountryExportAlloweds = await intraCountryExportAllowedRepository.GetAllAsync();
                if (destinationCompetentAuthority == sourceCompetentAuthority)
                {
                    transportRouteCopier.CopyTransportRoute(transportRoute, destinationTransportRoute, intraCountryExportAlloweds);
                }
                else
                {
                    transportRouteCopier.CopyTransportRouteWithoutExport(transportRoute, destinationTransportRoute, intraCountryExportAlloweds);
                }
            }

            context.TransportRoutes.Add(destinationTransportRoute);

            await context.SaveChangesAsync();

            return destinationTransportRoute;
        }

        private async Task CloneTechnologyEmployed(Guid sourceNotificationId, Guid destinationNotificationId)
        {
            var technologyEmployed =
                await context.TechnologiesEmployed.SingleOrDefaultAsync(p => p.NotificationId == sourceNotificationId);

            if (technologyEmployed != null)
            {
                TechnologyEmployed destinationTechnologyEmployed;

                if (technologyEmployed.AnnexProvided)
                {
                    destinationTechnologyEmployed =
                        TechnologyEmployed.CreateTechnologyEmployedWithAnnex(destinationNotificationId,
                            technologyEmployed.Details);
                }
                else
                {
                    destinationTechnologyEmployed =
                        TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails(
                            destinationNotificationId,
                            technologyEmployed.Details,
                            technologyEmployed.FurtherDetails);
                }

                context.TechnologiesEmployed.Add(destinationTechnologyEmployed);

                await context.SaveChangesAsync();
            }
        }
    }
}