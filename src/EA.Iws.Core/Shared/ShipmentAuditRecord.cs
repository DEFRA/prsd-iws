namespace EA.Iws.Core.Shared
{
    using System;
    using Movement;

    public class ShipmentAuditRecord
    {
        public ShipmentAuditRecord()
        {
        }

        public ShipmentAuditRecord(int shipmentNumber, MovementAuditType auditType, string userName,  DateTimeOffset dateAdded, string internalExternal)
        {
            UserName = userName;
            ShipmentNumber = shipmentNumber;
            AuditType = auditType;
            DateAdded = dateAdded;
            InternalExternal = internalExternal;
        }

        public string UserName { get; set; }

        public int ShipmentNumber { get; set; }

        public MovementAuditType AuditType { get; set; }

        public DateTimeOffset DateAdded { get; set; }
        public string InternalExternal { get; set; }
    }
}
