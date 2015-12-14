namespace EA.Iws.Domain.ImportNotification.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteCodes;

    public class EwcCode
    {
        public IEnumerable<WasteCode> Codes { get; private set; }

        private EwcCode(IEnumerable<WasteCode> codes)
        {
            if (codes.Any(c => c.Type != CodeType.Ewc))
            {
                throw new ArgumentException("Not all supplied waste codes are of type EWC");
            }

            Codes = codes;
        }

        public static EwcCode CreateFor(IEnumerable<WasteCode> codes)
        {
            return new EwcCode(codes);
        }
    }
}