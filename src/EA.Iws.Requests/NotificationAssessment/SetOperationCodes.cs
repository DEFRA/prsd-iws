namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using EA.Iws.Core.OperationCodes;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditOperationCodes)]
    public class SetOperationCodes : IRequest<bool>
    {
        public SetOperationCodes(Guid notificationId, IList<OperationCode> operationCodes)
        {
            NotificationId = notificationId;
            OperationCodes = operationCodes;
        }

        public Guid NotificationId { get; private set; }
        public IList<OperationCode> OperationCodes { get; private set; }
    }
}
