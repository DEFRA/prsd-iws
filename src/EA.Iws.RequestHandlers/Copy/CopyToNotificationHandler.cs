namespace EA.Iws.RequestHandlers.Copy
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Copy;

    internal class CopyToNotificationHandler : IRequestHandler<CopyToNotification, Guid>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly NotificationToNotificationCopy copier;

        public CopyToNotificationHandler(IwsContext context,
            IUserContext userContext,
            NotificationToNotificationCopy copier)
        {
            this.context = context;
            this.userContext = userContext;
            this.copier = copier;
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
            var destination = await context.NotificationApplications.SingleAsync(n => n.Id == destinationId);

            var clone = await GetCopyOfSourceNotification(sourceId);

            copier.CopyNotificationProperties(clone, destination);

            context.NotificationApplications.Add(clone);

            // Remove the destination.
            context.NotificationApplications.Remove(destination);

            // Save.
            await context.SaveChangesAsync();

            // Set child objects which have lookup properties. These can't be copied by the
            // main copy process due to detaching the change tracker.
            var source = await context.NotificationApplications.SingleAsync(n => n.Id == sourceId);
            copier.CopyLookupEntities(source, clone);

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
                        .Include(n => n.Exporter)
                        .Include("ProducersCollection")
                        .Include(n => n.Importer)
                        .Include("FacilitiesCollection")
                        .Include("OperationInfosCollection")
                        .Include(n => n.TechnologyEmployed)
                        .Include("CarriersCollection")
                        .Include("PackagingInfosCollection")
                        .Include("WasteType.WasteCompositionCollection")
                        .Include("WasteType.WasteAdditionalInformationCollection")
                        .Include("PhysicalCharacteristicsCollection")
                        .Include(n => n.RecoveryInfo)
                        .SingleAsync(n => n.Id == id);

            return clone;
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
    }
}