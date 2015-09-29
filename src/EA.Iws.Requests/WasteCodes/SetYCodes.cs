namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using System.Collections.Generic;

    [NotificationReadOnlyAuthorize]
    public class SetYCodes : BaseSetCodes
    {
        public SetYCodes(Guid id, IEnumerable<Guid> codes, bool isNotApplicable) 
            : base(id, codes, isNotApplicable)
        {
        }
    }
}
