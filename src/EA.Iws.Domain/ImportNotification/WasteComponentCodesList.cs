namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;

    public class WasteComponentCodesList : IEnumerable<WasteComponentCode>
    {
        private readonly List<WasteComponentCode> wasteComponentCodes;

        private WasteComponentCodesList(IEnumerable<WasteComponentCode> wasteComponentCodes)
        {
            this.wasteComponentCodes = new List<WasteComponentCode>(wasteComponentCodes.Select(c => new WasteComponentCode(c)));
        }

        public static WasteComponentCodesList CreateForNotification(ImportNotification notification, IEnumerable<WasteComponentCode> wasteComponentCodes)
        {
            var codes = wasteComponentCodes as WasteComponentCode[] ?? wasteComponentCodes.ToArray();

            if (!codes.IsUnique())
            {
                throw new ArgumentException("Waste component codes must be unique", "WasteComponentCodes");
            }

            return new WasteComponentCodesList(codes);
        }

        public IEnumerator<WasteComponentCode> GetEnumerator()
        {
            return wasteComponentCodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return wasteComponentCodes.GetEnumerator();
        }
    }
}
