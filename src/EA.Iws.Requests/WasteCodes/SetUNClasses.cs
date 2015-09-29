namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using System.Collections.Generic;

    [NotificationReadOnlyAuthorize]
    public class SetUNClasses : BaseSetCodes
    {
        public SetUNClasses(Guid id, IEnumerable<Guid> codes, bool isNotApplicable) 
            : base(id, codes, isNotApplicable)
        {
        }
    }
}
