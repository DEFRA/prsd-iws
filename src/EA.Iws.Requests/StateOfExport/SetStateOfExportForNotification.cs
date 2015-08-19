namespace EA.Iws.Requests.StateOfExport
{
    using System;
    using Prsd.Core.Mediator;

    public class SetStateOfExportForNotification : IRequest<Guid>
    {
        public Guid NotificationId { get; private set; }

        public Guid EntryOrExitPointId { get; private set; }

        public SetStateOfExportForNotification(Guid notificationId, Guid entryOrExitPointId)
        {
            NotificationId = notificationId;
            EntryOrExitPointId = entryOrExitPointId;
        }
    }
}
