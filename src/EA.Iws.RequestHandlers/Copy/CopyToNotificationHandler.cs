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
                    var destination = await context.NotificationApplications.SingleAsync(n => n.Id == message.DestinationId);

                    var clone = await GetCopyOfSourceNotification(message.SourceId);

                    SetCopyNotificationProperties(clone, destination);

                    context.NotificationApplications.Add(clone);

                    // Remove the destination.
                    context.NotificationApplications.Remove(destination);

                    // Save.
                    await context.SaveChangesAsync();

                    // Now change tracking is enabled clone the entities with lookup properties.
                    await SetCopyChildObjects(clone, message.SourceId);

                    if (GetChangedEntities(EntityState.Deleted).Any())
                    {
                        throw new InvalidOperationException(string.Format("The copy operation from notification {0} to {1} failed because {2}",
                            message.SourceId,
                            destination.Id,
                            " the operation would have deleted child entities."));
                    }

                    await context.SaveChangesAsync();

                    // Check we have not created a duplicate notification number.
                    var notificationCount = await context.NotificationApplications
                        .Select(n => n.NotificationNumber)
                        .GroupBy(n => n).CountAsync();

                    if (notificationCount != context.NotificationApplications.Count())
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

        private void SetCopyNotificationProperties(NotificationApplication clone, NotificationApplication destination)
        {
            // We want to set all properties except a few decided by business logic.
            typeof(NotificationApplication).GetProperty("NotificationNumber").SetValue(clone, destination.NotificationNumber, null);
            typeof(NotificationApplication).GetProperty("CompetentAuthority").SetValue(clone, destination.CompetentAuthority, null);
            typeof(NotificationApplication).GetProperty("NotificationType").SetValue(clone, destination.NotificationType, null);

            // This should not be needed however is a precaution to prevent overwriting the source data.
            typeof(Entity).GetProperty("Id").SetValue(clone, Guid.Empty, null);
        }

        /// <summary>
        /// Set child objects which have nested child objects. These can't be copied by the
        /// main copy process due to detaching the change tracker.
        /// </summary>
        /// <param name="clone"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        private async Task SetCopyChildObjects(NotificationApplication clone, Guid sourceId)
        {
            var source = await context.NotificationApplications.SingleAsync(n => n.Id == sourceId);
            copier.CopyLookupEntities(source, clone);
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
