namespace EA.Iws.Core.NotificationAssessment
{
    using System;

    public class ConfirmNumberOfShipmentsChangeData
    {
        public int CurrentNumberOfShipments { get; set; }

        public decimal CurrentCharge { get; set; }

        public decimal NewCharge { get; set; }

        public Guid NotificationId { get; set; }
    }
}
