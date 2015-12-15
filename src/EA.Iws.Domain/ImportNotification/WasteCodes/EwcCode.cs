namespace EA.Iws.Domain.ImportNotification.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteCodes;
    using NotificationApplication;

    public class EwcCode
    {
        public IEnumerable<WasteTypeWasteCode> Codes { get; private set; }

        private EwcCode(IEnumerable<WasteCode> codes)
        {
            if (codes.Any(c => c.CodeType != CodeType.Ewc))
            {
                throw new ArgumentException("Not all supplied waste codes are of type EWC");
            }

            Codes = codes.Select(c => new WasteTypeWasteCode(c.Id));
        }

        public static EwcCode CreateFor(IEnumerable<WasteCode> codes)
        {
            return new EwcCode(codes);
        }
    }
}