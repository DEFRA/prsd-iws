namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportNotificationAssessment)]
    public class SetNotificationFileClosedDate : IRequest<Unit>
    {
        public Guid ImportNotificationId { get; private set; }

        public DateTime Date { get; private set; }

        public SetNotificationFileClosedDate(Guid importNotificationId, DateTime date)
        {
            ImportNotificationId = importNotificationId;
            Date = date;
        }
    }
}
