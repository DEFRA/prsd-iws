namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using Security;

    [NotificationReadOnlyAuthorize]
    public class SetEwcCodes : BaseSetCodes
    {
        public SetEwcCodes(Guid id, IEnumerable<Guid> codes) 
            : base(id, codes, isNotApplicable: false)
        {
        }
    }
}
