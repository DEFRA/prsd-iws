namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.NumberOfShipments
{
    using System;
    using Core.NotificationAssessment;

    public class ConfirmViewModel
    {
        public int NewNumberOfShipments { get; set; }

        public int OldNumberOfShipments { get; set; }

        public Guid NotificationId { get; set; }

        public decimal CurrentCharge { get; set; }

        public decimal NewCharge { get; set; }

        public ConfirmViewModel(ConfirmNumberOfShipmentsChangeData data)
        {
            NotificationId = data.NotificationId;
            CurrentCharge = data.CurrentCharge;
            OldNumberOfShipments = data.CurrentNumberOfShipments;
            NewCharge = data.NewCharge;
        }

        public bool IsIncrease
        {
            get { return NewNumberOfShipments > OldNumberOfShipments; }
        }

        public decimal IncreaseInCharge
        {
            get { return NewCharge - CurrentCharge; }
        }
    }
}