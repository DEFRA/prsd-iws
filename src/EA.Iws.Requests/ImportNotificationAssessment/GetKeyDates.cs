namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotificationAssessment)]
    public class GetKeyDates : IRequest<KeyDatesData>
    {
        public Guid ImportNotificationId { get; private set; }

        public GetKeyDates(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}
