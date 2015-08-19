namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Prsd.Core.Mediator;

    public class SetYCodes : IRequest<bool>
    {
        public Guid Id { get; private set; }

        public IEnumerable<Guid> Codes { get; private set; }

        public bool IsNotApplicable { get; private set; }

        public SetYCodes(Guid id, IEnumerable<Guid> codes, bool isNotApplicable)
        {
            if (isNotApplicable && codes != null && codes.Any())
            {
                throw new InvalidOperationException("Cannot set not applicable and codes for EWC codes.");
            }

            Id = id;
            Codes = codes;
            IsNotApplicable = isNotApplicable;
        }
    }
}
