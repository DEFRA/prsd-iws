namespace EA.Iws.Core.Shared
{
    using System;
    using Movement;

    public class ShipmentAuditRecord
    {
        public ShipmentAuditRecord()
        {
        }

        public ShipmentAuditRecord(int shipmentNumber, MovementAuditType auditType, string userName,  DateTimeOffset dateAdded)
        {
            UserName = userName;
            ShipmentNumber = shipmentNumber;
            AuditType = auditType;
            DateAdded = dateAdded;
        }

        public string UserName { get; set; }

        public int ShipmentNumber { get; set; }

        public MovementAuditType AuditType { get; set; }

        public DateTimeOffset DateAdded { get; set; }
    }
}
