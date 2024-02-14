namespace EA.Iws.Domain.NotificationApplication
{
    using EA.Iws.Core.Notification.AdditionalCharge;
    using System;

    public class AdditionalCharge
    {
        protected AdditionalCharge()
        {
        }

        public AdditionalCharge(Guid id, Guid notificationId, DateTime chargeDate, decimal chargeAmount, int type, string comments)
        {
            this.Id = id;
            NotificationId = notificationId;
            ChargeDate = chargeDate;
            ChargeAmount = chargeAmount;
            ChangeDetailType = (AdditionalChargeType)type;
            Comments = comments;
        }

        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }

        public DateTime ChargeDate { get; set; }

        public decimal ChargeAmount { get; set; }

        public AdditionalChargeType ChangeDetailType { get; set; }

        public string Comments { get; set; }
    }
}
