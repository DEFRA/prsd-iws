namespace EA.Iws.Core.Notification.AdditionalCharge
{
    using EA.Iws.Core.Shared;
    using System;

    public class NotificationAdditionalChargeForDisplay
    {
        public NotificationAdditionalChargeForDisplay()
        {
        }

        public NotificationAdditionalChargeForDisplay(DateTime chargeDate, decimal chargeAmount, AdditionalChargeType changeDetailType, string comments)
        {
            this.ChargeDate = chargeDate;
            this.ChargeAmount = chargeAmount;
            this.ChangeDetailType = changeDetailType;
            this.Comments = comments;
        }

        public DateTime ChargeDate { get; set; }

        public decimal ChargeAmount { get; set; }

        public AdditionalChargeType ChangeDetailType { get; set; }

        public string Comments { get; set; }
    }
}
