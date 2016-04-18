namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotification.Summary;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotification)]
    public class GetSummary : IRequest<ImportNotificationSummary>
    {
        public Guid Id { get; set; }

        public GetSummary(Guid id)
        {
            Id = id;
        }
    }
}
