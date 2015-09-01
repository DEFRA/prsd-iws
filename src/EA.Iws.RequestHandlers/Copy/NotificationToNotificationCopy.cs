namespace EA.Iws.RequestHandlers.Copy
{
    using System;
    using System.Linq;
    using Domain.NotificationApplication;
    using Domain.TransportRoute;
    using Prsd.Core.Domain;

    internal class NotificationToNotificationCopy
    {
        private readonly WasteCodeCopy wasteCodeCopy;

        public NotificationToNotificationCopy(WasteCodeCopy wasteCodeCopy)
        {
            this.wasteCodeCopy = wasteCodeCopy;
        }

        public virtual void CopyNotificationProperties(NotificationApplication source, NotificationApplication destination)
        {
            // We want to set all properties except a few decided by business logic.
            typeof(NotificationApplication).GetProperty("NotificationNumber").SetValue(source, destination.NotificationNumber, null);
            typeof(NotificationApplication).GetProperty("CompetentAuthority").SetValue(source, destination.CompetentAuthority, null);
            typeof(NotificationApplication).GetProperty("NotificationType").SetValue(source, destination.NotificationType, null);

            // This should not be needed however is a precaution to prevent overwriting the source data.
            typeof(Entity).GetProperty("Id").SetValue(source, Guid.Empty, null);
        }

        public virtual void CopyLookupEntities(NotificationApplication source, NotificationApplication destination)
        {
            CopyStateOfExport(source, destination);
            CopyStateOfImport(source, destination);
            CopyTransitStates(source, destination);
            CopyCustomsOffices(source, destination);
            CopyWasteCodes(source, destination);
        }

        protected virtual void CopyStateOfExport(NotificationApplication source, NotificationApplication destination)
        {
            destination.SetStateOfExportForNotification(new StateOfExport(source.StateOfExport.Country,
                source.StateOfExport.CompetentAuthority,
                source.StateOfExport.ExitPoint));
        }

        protected virtual void CopyStateOfImport(NotificationApplication source, NotificationApplication destination)
        {
            destination.SetStateOfImportForNotification(new StateOfImport(source.StateOfImport.Country,
                source.StateOfImport.CompetentAuthority,
                source.StateOfImport.EntryPoint));
        }

        protected virtual void CopyTransitStates(NotificationApplication source, NotificationApplication destination)
        {
            foreach (var transitState in source.TransitStates.OrderBy(ts => ts.OrdinalPosition))
            {
                destination.AddTransitStateToNotification(new TransitState(transitState.Country,
                    transitState.CompetentAuthority,
                    transitState.EntryPoint,
                    transitState.ExitPoint,
                    transitState.OrdinalPosition));
            }
        }

        protected virtual void CopyCustomsOffices(NotificationApplication source, NotificationApplication destination)
        {
            if (source.EntryCustomsOffice != null)
            {
                destination.SetEntryCustomsOffice(new EntryCustomsOffice(source.EntryCustomsOffice.Name,
                    source.EntryCustomsOffice.Address,
                    source.EntryCustomsOffice.Country));
            }

            if (source.ExitCustomsOffice != null)
            {
                destination.SetExitCustomsOffice(new ExitCustomsOffice(source.ExitCustomsOffice.Name,
                    source.ExitCustomsOffice.Address,
                    source.ExitCustomsOffice.Country));
            }
        }

        protected virtual void CopyWasteCodes(NotificationApplication source, NotificationApplication destination)
        {
            wasteCodeCopy.CopyWasteCodes(source, destination);
        }
    }
}
