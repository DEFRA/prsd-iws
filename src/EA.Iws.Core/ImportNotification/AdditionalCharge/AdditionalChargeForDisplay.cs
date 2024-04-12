namespace EA.Iws.Core.ImportNotification.AdditionalCharge
{
    using EA.Iws.Core.Notification.AdditionalCharge;
    using System;

    public class AdditionalChargeForDisplay
    {
        public AdditionalChargeForDisplay()
        {
        }

        public AdditionalChargeForDisplay(DateTime chargeDate, decimal chargeAmount, AdditionalChargeType changeDetailType, string comments)
        {
            ChargeDate = chargeDate;
            ChargeAmount = chargeAmount;
            ChangeDetailType = changeDetailType;
            Comments = comments;
        }

        public DateTime ChargeDate { get; set; }

        public decimal ChargeAmount { get; set; }

        public AdditionalChargeType ChangeDetailType { get; set; }

        public string Comments { get; set; }
    }
}
