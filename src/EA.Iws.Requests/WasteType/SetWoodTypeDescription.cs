namespace EA.Iws.Requests.WasteType
{
    using System;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    public class SetWoodTypeDescription : IRequest<Guid>
    {
        public SetWoodTypeDescription(string woodTypeDescription, Guid notificationId)
        {
            WoodTypeDescription = woodTypeDescription;
            NotificationId = notificationId;
        }

        public string WoodTypeDescription { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}