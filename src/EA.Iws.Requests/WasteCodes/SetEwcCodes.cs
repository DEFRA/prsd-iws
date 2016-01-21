namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Security;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetEwcCodes : BaseSetCodes
    {
        public SetEwcCodes(Guid id, IEnumerable<Guid> codes) 
            : base(id, codes, isNotApplicable: false)
        {
        }
    }
}
