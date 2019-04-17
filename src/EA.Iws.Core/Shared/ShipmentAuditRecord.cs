namespace EA.Iws.Core.Shared
{
    using System;

    public class ShipmentAuditRecord
    {
        public ShipmentAuditRecord()
        {
        }

        public ShipmentAuditRecord(int shipmentNumber, string auditType, string userName,  DateTimeOffset dateAdded)
        {
            this.UserName = userName;
            this.ShipmentNumber = shipmentNumber;
            this.AuditType = auditType;
            this.DateAdded = dateAdded;
        }

        public string UserName { get; set; }

        public int ShipmentNumber { get; set; }

        public string AuditType { get; set; }

        public DateTimeOffset DateAdded { get; set; }
    }
}
