namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.StateOfImport;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanChangeImportEntryExitPoint)]
    public class GetStateOfImportData : IRequest<StateOfImportData>
    {
        public GetStateOfImportData(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        public Guid ImportNotificationId { get; private set; }
    }
}