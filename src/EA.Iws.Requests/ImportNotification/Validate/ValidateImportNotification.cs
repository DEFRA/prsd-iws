namespace EA.Iws.Requests.ImportNotification.Validate
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanCompleteImportNotification)]
    public class ValidateImportNotification : IRequest<IEnumerable<ValidationResults>>
    {
        public ValidateImportNotification(Guid draftImportNotificationId)
        {
            DraftImportNotificationId = draftImportNotificationId;
        }

        public Guid DraftImportNotificationId { get; private set; }
    }
}