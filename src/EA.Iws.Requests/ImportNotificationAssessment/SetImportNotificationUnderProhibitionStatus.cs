namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportNotificationAssessment)]
    public class SetImportNotificationUnderProhibitionStatus : IRequest<bool>
    {
        public Guid ImportNotificationId { get; private set; }

        public DateTime UnderProhibitionDate { get; private set; }

        public SetImportNotificationUnderProhibitionStatus(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
            UnderProhibitionDate = DateTime.UtcNow;
        }
    }
}
