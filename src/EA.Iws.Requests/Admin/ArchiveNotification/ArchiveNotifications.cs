namespace EA.Iws.Requests.Admin.ArchiveNotification
{
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using EA.Iws.Core.Admin.ArchiveNotification;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanViewSearchResults)]

    public class ArchiveNotifications : IRequest<IList<ArchiveNotificationResult>>
    {
        public ArchiveNotifications()
        {            
        }
    }
}
