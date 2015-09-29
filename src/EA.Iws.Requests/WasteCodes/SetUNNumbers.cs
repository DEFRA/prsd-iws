namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using System.Collections.Generic;

    [NotificationReadOnlyAuthorize]
    public class SetUNNumbers : BaseSetCodes
    {
        public SetUNNumbers(Guid id, IEnumerable<Guid> codes, bool isNotApplicable) 
            : base(id, codes, isNotApplicable)
        {
        }
    }
}