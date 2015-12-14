namespace EA.Iws.Domain.ImportNotification.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteCodes;

    public class UnClass
    {
        public bool NotApplicable { get; private set; }

        public IEnumerable<WasteCode> Codes { get; private set; }

        private UnClass()
        {
        }
        
        private UnClass(IEnumerable<WasteCode> codes)
        {
            if (codes.Any(c => c.Type != CodeType.Un))
            {
                throw new ArgumentException("Not all supplied waste codes are of type UN class");
            }

            Codes = codes;
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