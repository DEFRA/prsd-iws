namespace EA.Iws.Requests.OperationCodes
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core.Mediator;

    public class GetOperationCodesByNotificationId : IRequest<IList<OperationCodeData>>
    {
        public Guid NotificationId { get; set; }

        public GetOperationCodesByNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
