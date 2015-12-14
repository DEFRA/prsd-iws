namespace EA.Iws.Domain.ImportNotification.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteCodes;

    public class HCode
    {
        public bool NotApplicable { get; private set; }

        public IEnumerable<WasteCode> Codes { get; private set; }

        private HCode()
        {
        }

        private HCode(IEnumerable<WasteCode> codes)
        {
            if (codes.Any(c => c.Type != CodeType.H))
            {
                throw new ArgumentException("Not all supplied waste codes are of type H");
            }

            Codes = codes;
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