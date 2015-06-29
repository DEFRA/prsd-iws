namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Prsd.Core.Mediator;

    public class SetUnNumberWasteCodes : IRequest<Guid>
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation",
            Justification = "Variable relates UN numbers")]
        public SetUnNumberWasteCodes(Guid notificationId, IEnumerable<Guid> unNumbers, IEnumerable<string> customsCodes)
        {
            NotificationId = notificationId;
            UnNumbers = unNumbers;
            CustomsCodes = customsCodes;
        }

        public Guid NotificationId { get; private set; }

        public IEnumerable<Guid> UnNumbers { get; private set; }

        public IEnumerable<string> CustomsCodes { get; private set; }
    }
}