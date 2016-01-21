namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportNotification)]
    public class SetDraftData<TData> : IRequest<bool>
    {
        public SetDraftData(Guid importNotificationId, TData data)
        {
            ImportNotificationId = importNotificationId;
            Data = data;
        }

        public Guid ImportNotificationId { get; private set; }

        public TData Data { get; private set; }
    }
}