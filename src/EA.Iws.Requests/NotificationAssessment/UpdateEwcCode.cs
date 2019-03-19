namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using WasteCodes;

    [RequestAuthorization(ExportNotificationPermissions.CanEditEwcCodes)]
    public class UpdateEwcCode : BaseSetCodes
    {
        public UpdateEwcCode(Guid id, IEnumerable<Guid> codes)
            : base(id, codes, isNotApplicable: false)
        {
        }
    }
}
