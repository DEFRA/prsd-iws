namespace EA.Iws.Domain.ImportNotification.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteCodes;
    using NotificationApplication;

    public class YCode
    {
        public bool NotApplicable { get; private set; }

        public IEnumerable<WasteTypeWasteCode> Codes { get; private set; }

        private YCode()
        {
        }
        
        private YCode(IEnumerable<WasteCode> codes)
        {
            if (codes.Any(c => c.CodeType != CodeType.Y))
            {
                throw new ArgumentException("Not all supplied waste codes are of type Y");
            }

            Codes = codes.Select(c => new WasteTypeWasteCode(c.Id));
        }

        public static YCode CreateNotApplicable()
        {
            return new YCode
            {
                NotApplicable = true
            };
        }

        public static YCode CreateFor(IEnumerable<WasteCode> codes)
        {
            return new YCode(codes);
        }
    }
}