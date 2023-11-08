namespace EA.Iws.Domain.ImportNotification
{
    using EA.Prsd.Core;
    using EA.Prsd.Core.Domain;
    using EA.Prsd.Core.Extensions;
    using System;
    using System.Collections.Generic;

    public class WasteComponent : Entity
    {
        protected WasteComponent()
        {
        }

        public WasteComponent(Guid importNotificationId, WasteComponentCodesList wasteComponentCodes)
        {
            Guard.ArgumentNotDefaultValue(() => importNotificationId, importNotificationId);
            Guard.ArgumentNotNull(() => wasteComponentCodes, wasteComponentCodes);

            ImportNotificationId = importNotificationId;
            ComponentCodesCollection = new List<WasteComponentCode>(wasteComponentCodes);
        }

        public Guid ImportNotificationId { get; private set; }

        protected virtual ICollection<WasteComponentCode> ComponentCodesCollection { get; set; }        

        public IEnumerable<WasteComponentCode> Codes
        {
            get { return ComponentCodesCollection.ToSafeIEnumerable(); }
        }

        public void SetComponentCodes(List<WasteComponentCode> wasteComponentCodes)
        {
            Guard.ArgumentNotNull(() => wasteComponentCodes, wasteComponentCodes);

            ComponentCodesCollection.Clear();

            ComponentCodesCollection = wasteComponentCodes;
        }
    }
}
