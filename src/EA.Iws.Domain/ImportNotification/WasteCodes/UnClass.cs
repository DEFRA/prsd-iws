namespace EA.Iws.Domain.ImportNotification.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteCodes;
    using NotificationApplication;

    public class UnClass
    {
        public bool NotApplicable { get; private set; }

        public IEnumerable<WasteTypeWasteCode> Codes { get; private set; }

        private UnClass()
        {
        }

        private UnClass(IEnumerable<WasteCode> codes)
        {
            if (codes.Any(c => c.CodeType != CodeType.Un))
            {
                throw new ArgumentException("Not all supplied waste codes are of type UN class");
            }

            Codes = codes.Select(c => new WasteTypeWasteCode(c.Id));
        }

        public static UnClass CreateNotApplicable()
        {
            return new UnClass
            {
                NotApplicable = true
            };
        }

        public static UnClass CreateFor(IEnumerable<WasteCode> codes)
        {
            return new UnClass(codes);
        }
    }
}