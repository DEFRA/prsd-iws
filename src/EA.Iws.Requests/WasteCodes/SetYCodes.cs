namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetYCodes : BaseSetCodes
    {
        public SetYCodes(Guid id, IEnumerable<Guid> codes, bool isNotApplicable)
            : base(id, codes, isNotApplicable)
        {
        }
    }
}