namespace EA.Iws.Requests.ImportNotification
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.ImportNotification.AdditionalCharge;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Collections.Generic;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotification)]
    public class GetImportNotificationAdditionalCharges : IRequest<IList<AdditionalChargeForDisplay>>
    {
        public GetImportNotificationAdditionalCharges(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}
