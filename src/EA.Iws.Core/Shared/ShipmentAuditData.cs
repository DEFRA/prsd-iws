namespace EA.Iws.Core.Shared
{
    using System.Collections.Generic;

    public class ShipmentAuditData
    {
        public List<ShipmentAuditRecord> TableData { get; set; }

        public int NumberOfShipmentAudits { get; set; }
        public int NumberOfFilteredShipmentAudits { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
