namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Prsd.Core.Mediator;

    public class SetYHUnWasteCodes : IRequest<Guid>
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation",
            Justification = "Variables relate to Y/H/UN-Codes")]
        public SetYHUnWasteCodes(Guid notificationId, IEnumerable<Guid> yCodes, IEnumerable<Guid> hCodes,
            IEnumerable<Guid> unCodes)
        {
            NotificationId = notificationId;
            YCodes = yCodes;
            HCodes = hCodes;
            UnCodes = unCodes;
        }

        public Guid NotificationId { get; private set; }

        public IEnumerable<Guid> YCodes { get; private set; }

        public IEnumerable<Guid> HCodes { get; private set; }

        public IEnumerable<Guid> UnCodes { get; private set; }
    }
}