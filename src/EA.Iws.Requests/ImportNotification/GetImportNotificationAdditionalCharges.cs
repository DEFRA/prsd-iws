namespace EA.Iws.Requests.ImportNotification
{
    using EA.Iws.Core.ImportNotification.AdditionalCharge;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Collections.Generic;

    public class GetImportNotificationAdditionalCharges : IRequest<IList<AdditionalChargeForDisplay>>
    {
        public GetImportNotificationAdditionalCharges(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}
