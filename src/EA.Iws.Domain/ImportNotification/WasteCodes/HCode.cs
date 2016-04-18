namespace EA.Iws.Domain.ImportNotification.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteCodes;
    using NotificationApplication;

    public class HCode
    {
        public bool NotApplicable { get; private set; }

        public IEnumerable<WasteTypeWasteCode> Codes { get; private set; }

        private HCode()
        {
        }

        private HCode(IEnumerable<WasteCode> codes)
        {
            if (codes.Any(c => c.CodeType != CodeType.H))
            {
                throw new ArgumentException("Not all supplied waste codes are of type H");
            }

            Codes = codes.Select(c => new WasteTypeWasteCode(c.Id));
        }

        public static HCode CreateNotApplicable()
        {
            return new HCode
            {
                NotApplicable = true
            };
        }

        public static HCode CreateFor(IEnumerable<WasteCode> codes)
        {
            return new HCode(codes);
        }
    }
}