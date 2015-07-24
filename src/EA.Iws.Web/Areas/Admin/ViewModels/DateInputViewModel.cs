namespace EA.Iws.Web.Areas.Admin.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DateInputViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        //----------------Notification Received-----------

        [Display(Name = "Day")]
        public int? NotificationReceivedDay { get; set; }

        [Display(Name = "Month")]
        public int? NotificationReceivedMonth { get; set; }

        [Display(Name = "Year")]
        public int? NotificationReceivedYear { get; set; }

        public bool NotificationReceivedComplete
        {
            get { return DateComplete(NotificationReceivedDay, NotificationReceivedMonth, NotificationReceivedYear); }
        }

        //----------------Payment Received-----------

        [Display(Name = "Day")]
        public int? PaymentReceivedDay { get; set; }

        [Display(Name = "Month")]
        public int? PaymentReceivedMonth { get; set; }

        [Display(Name = "Year")]
        public int? PaymentReceivedYear { get; set; }

        public bool PaymentReceivedComplete
        {
            get { return DateComplete(PaymentReceivedDay, PaymentReceivedMonth, PaymentReceivedYear); }
        }

        //----------------Commencement-----------

        [Display(Name = "Day")]
        public int? CommencementDay { get; set; }

        [Display(Name = "Month")]
        public int? CommencementMonth { get; set; }

        [Display(Name = "Year")]
        public int? CommencementYear { get; set; }

        [Display(Name = "Name of officer")]
        public string NameOfOfficer { get; set; }

        public bool CommencementComplete
        {
            get { return (DateComplete(CommencementDay, CommencementMonth, CommencementYear) && !string.IsNullOrWhiteSpace(NameOfOfficer)); }
        }

        //----------------Notification Complete-----------

        [Display(Name = "Day")]
        public int? NotificationCompleteDay { get; set; }

        [Display(Name = "Month")]
        public int? NotificationCompleteMonth { get; set; }

        [Display(Name = "Year")]
        public int? NotificationCompleteYear { get; set; }

        public bool NotificationCompleteComplete
        {
            get { return DateComplete(NotificationCompleteDay, NotificationCompleteMonth, NotificationCompleteYear); }
        }

        //----------------Notification Transmitted-----------

        [Display(Name = "Day")]
        public int? NotificationTransmittedDay { get; set; }

        [Display(Name = "Month")]
        public int? NotificationTransmittedMonth { get; set; }

        [Display(Name = "Year")]
        public int? NotificationTransmittedYear { get; set; }

        public bool NotificationTransmittedComplete
        {
            get { return DateComplete(NotificationTransmittedDay, NotificationTransmittedMonth, NotificationTransmittedYear); }
        }

        //----------------Notification Acknowledged -----------

        [Display(Name = "Day")]
        public int? NotificationAcknowledgedDay { get; set; }

        [Display(Name = "Month")]
        public int? NotificationAcknowledgedMonth { get; set; }

        [Display(Name = "Year")]
        public int? NotificationAcknowledgedYear { get; set; }

        public bool NotificationAcknowledgedComplete
        {
            get { return DateComplete(NotificationAcknowledgedDay, NotificationAcknowledgedMonth, NotificationAcknowledgedYear); }
        }

        //----------------Decision Date-----------

        [Display(Name = "Day")]
        public int? DecisionDay { get; set; }

        [Display(Name = "Month")]
        public int? DecisionMonth { get; set; }

        [Display(Name = "Year")]
        public int? DecisionYear { get; set; }

        public bool DecsionDateComplete
        {
            get { return DateComplete(DecisionDay, DecisionMonth, DecisionYear); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ValidateDate(NotificationReceivedDay, NotificationReceivedMonth, NotificationReceivedYear))
            {
                yield return new ValidationResult("Please provide a valid notification received date", new[] { "NotificationReceivedDay" });                
            }

            if (!ValidateDate(PaymentReceivedDay, PaymentReceivedMonth, PaymentReceivedYear))
            {
                yield return new ValidationResult("Please provide a valid payment received date", new[] { "PaymentReceivedDay" });
            }

            if (!ValidateDate(CommencementDay, CommencementMonth, CommencementYear))
            {
                yield return new ValidationResult("Please provide a valid assessment commencement date", new[] { "CommencementDay" });
            }

            if (!ValidateDate(NotificationCompleteDay, NotificationCompleteMonth, NotificationCompleteYear))
            {
                yield return new ValidationResult("Please provide a valid notification complete date", new[] { "NotificationReceivedDay" });
            }

            if (!ValidateDate(NotificationTransmittedDay, NotificationTransmittedMonth, NotificationTransmittedYear))
            {
                yield return new ValidationResult("Please provide a valid notification transmitted date", new[] { "NotificationTransmittedDay" });
            }

            if (!ValidateDate(NotificationAcknowledgedDay, NotificationAcknowledgedMonth, NotificationAcknowledgedYear))
            {
                yield return new ValidationResult("Please provide a valid notification acknowledged date", new[] { "NotificationAcknowledgedDay" });
            }

            if (!ValidateDate(DecisionDay, DecisionMonth, DecisionYear))
            {
                yield return new ValidationResult("Please provide a valid decision required date", new[] { "DecisionDay" });
            }
        }

        private bool ValidateDate(int? day, int? month, int? year)
        {
            if (day.HasValue || month.HasValue || year.HasValue)
            {
                if (!(day.HasValue && month.HasValue && year.HasValue))
                {
                    return false;
                }
                return day.Value <= 31 && day.Value >= 1 && month.Value <= 12 && month.Value >= 1 && year.Value <= 99999 && year.Value >= 1900;
            }
            return true;
        }

        private bool DateComplete(int? day, int? month, int? year)
        {
            if ((day.HasValue || month.HasValue || year.HasValue) && (day.HasValue && month.HasValue && year.HasValue))
            {
                return day.Value <= 31 && day.Value >= 1 && month.Value <= 12 && month.Value >= 1 && year.Value <= 99999 && year.Value >= 1900;
            }
            return false;
        }
    }
}