namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotification.Update;
    using Core.OperationCodes;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanChangeWasteOperation)]
    public class UpdateWasteOperation : IRequest<Guid>
    {
        public Guid ImportNotificationId { get; private set; }

        public IList<OperationCode> OperationCodes { get; private set; }

        public string TechnologyEmployed { get; private set; }

        public UpdateWasteOperation(Guid importNotificationId, IList<OperationCode> operationCodes, string technologyEmployed)
        {
            ImportNotificationId = importNotificationId;
            OperationCodes = operationCodes;
            TechnologyEmployed = technologyEmployed;
        }
    }
}