namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;

    [RequestAuthorization(ExportNotificationPermissions.CanEditHCodes)]
    public class EditHCodes : BaseSetCodes
    {
        public EditHCodes(Guid id, IEnumerable<Guid> codes, bool isNotApplicable)
            : base(id, codes, isNotApplicable)
        {
        }
    }
}
