namespace EA.Iws.Domain.Movement.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Movement.Bulk;
    using Core.Shared;
    using Prsd.Core.Domain;

    public class DraftMovement : Entity
    {
        public DraftMovement(Guid draftMovementId, PrenotificationMovement movement)
        {
            BulkUploadId = draftMovementId;
            NotificationNumber = movement.NotificationNumber;
            ShipmentNumber = movement.ShipmentNumber;
            Quantity = movement.Quantity;
            Units = movement.Unit;
            Date = movement.ActualDateOfShipment;
            PackagingInfosCollection =
                movement.PackagingTypes.Select(p => new DraftPackagingInfo() { PackagingType = p }).ToList();
        }

        public Guid BulkUploadId { get; set; }

        public string NotificationNumber { get; set; }

        public int? ShipmentNumber { get; set; }

        public decimal? Quantity { get; set; }

        public ShipmentQuantityUnits? Units { get; set; }

        public DateTime? Date { get; set; }

        protected virtual ICollection<DraftPackagingInfo> PackagingInfosCollection { get; set; }
    }
}
