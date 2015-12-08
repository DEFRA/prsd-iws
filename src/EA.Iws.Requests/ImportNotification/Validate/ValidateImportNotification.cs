namespace EA.Iws.Requests.ImportNotification.Validate
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core.Mediator;

    public class ValidateImportNotification : IRequest<IEnumerable<ValidationResults>>
    {
        public ValidateImportNotification(Guid draftImportNotificationId)
        {
            DraftImportNotificationId = draftImportNotificationId;
        }

        public Guid DraftImportNotificationId { get; private set; }
    }
}