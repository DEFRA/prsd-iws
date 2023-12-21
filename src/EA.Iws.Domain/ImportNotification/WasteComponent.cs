namespace EA.Iws.Domain.ImportNotification
{
    using EA.Iws.Core.WasteComponentType;
    using EA.Prsd.Core;
    using EA.Prsd.Core.Domain;
    using System;

    public class WasteComponent : Entity
    {
        protected WasteComponent()
        {
        }

        public WasteComponent(Guid importNotificationId, WasteComponentType wasteComponentType)
        {
            Guard.ArgumentNotDefaultValue(() => importNotificationId, importNotificationId);

            ImportNotificationId = importNotificationId;
            WasteComponentType = wasteComponentType;
        }

        public Guid ImportNotificationId { get; private set; }

        public WasteComponentType WasteComponentType { get; set; }
    }
}
