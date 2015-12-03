namespace EA.Iws.Requests.ImportNotification.Validate
{
    using System;
    using Prsd.Core.Mediator;

    public class ValidateImportNotification : IRequest<string[]>
    {
        public ValidateImportNotification(Guid draftImportNotificationId)
        {
            DraftImportNotificationId = draftImportNotificationId;
        }

        public Guid DraftImportNotificationId { get; private set; }
    }
}