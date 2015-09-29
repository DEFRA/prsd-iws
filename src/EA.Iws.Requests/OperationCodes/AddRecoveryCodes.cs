namespace EA.Iws.Requests.OperationCodes
{
    using System;
    using System.Collections.Generic;
    using Core.OperationCodes;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    public class AddRecoveryCodes : IRequest<Guid>
    {
        public AddRecoveryCodes(List<RecoveryCode> recoveryCodes, Guid notificationId)
        {
            RecoveryCodes = recoveryCodes;
            NotificationId = notificationId;
        }

        public List<RecoveryCode> RecoveryCodes { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}
