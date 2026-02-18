namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportNotificationAssessment)]
    public class SetUnderProhibitionStatus : IRequest<bool>
    {
        public Guid ImportNotificationId { get; private set; }

        public DateTime UnderProhibitionDate { get; private set; }

        public SetUnderProhibitionStatus(Guid importNotificationId, DateTime underProhibitionDate)
        {
            ImportNotificationId = importNotificationId;
            UnderProhibitionDate = underProhibitionDate;
        }
    }
}
