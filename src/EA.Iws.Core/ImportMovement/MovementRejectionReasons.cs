namespace EA.Iws.Core.ImportMovement
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class MovementRejectionReasons : IEnumerable<string>
    {
        private readonly string[] reasons = new[]
        {
            "Illegal shipment",
            "Unplanned shipment",
            "An accident",
            "Site inspection",
            "Rejected by consignee",
            "Refused entry by competent authority",
            "Unauthorised shipment",
            "Waste not as specified",
            "Abandoned waste",
            "Unrecovered waste"
        };

        public IEnumerator<string> GetEnumerator()
        {
            return reasons.Cast<string>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return reasons.GetEnumerator();
        }
    }
}
