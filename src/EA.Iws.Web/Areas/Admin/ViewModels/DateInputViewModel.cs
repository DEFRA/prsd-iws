namespace EA.Iws.Web.Areas.Admin.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class DateInputViewModel
    {
        //----------------Notification Received-----------

        [Display(Name = "Day")]
        [Required]
        [Range(1, 31)]
        public int NotificationReceivedDay { get; set; }

        [Display(Name = "Month")]
        [Required]
        [Range(1, 12)]
        public int NotificationReceivedMonth { get; set; }

        [Display(Name = "Year")]
        [Required]
        [Range(0, 9999999)]
        public int NotificationReceivedYear { get; set; }

        //----------------Payment Received-----------
  
        [Display(Name = "Day")]
        [Required]
        [Range(1, 31)]
        public int PaymentReceivedDay { get; set; }

        [Display(Name = "Month")]
        [Required]
        [Range(1, 12)]
        public int PaymentReceivedMonth { get; set; }

        [Display(Name = "Year")]
        [Required]
        [Range(0, 9999999)]
        public int PaymentReceivedYear { get; set; }

        //----------------Commencement-----------

        [Display(Name = "Day")]
        [Required]
        [Range(1, 31)]
        public int CommencementDay { get; set; }

        [Display(Name = "Month")]
        [Required]
        [Range(1, 12)]
        public int CommencementMonth { get; set; }

        [Display(Name = "Year")]
        [Required]
        [Range(0, 9999999)]
        public int CommencementYear { get; set; }

        public string NameOfOfficer { get; set; }

        //----------------Notification Complete-----------

        [Display(Name = "Day")]
        [Required]
        [Range(1, 31)]
        public int NotificationCompleteDay { get; set; }

        [Display(Name = "Month")]
        [Required]
        [Range(1, 12)]
        public int NotificationCompleteMonth { get; set; }

        [Display(Name = "Year")]
        [Required]
        [Range(0, 9999999)]
        public int NotificationCompleteYear { get; set; }

        //----------------Notification Transmitted-----------

        [Display(Name = "Day")]
        [Required]
        [Range(1, 31)]
        public int NotificationTransmittedDay { get; set; }

        [Display(Name = "Month")]
        [Required]
        [Range(1, 12)]
        public int NotificationTransmittedMonth { get; set; }

        [Display(Name = "Year")]
        [Required]
        [Range(0, 9999999)]
        public int NotificationTransmittedYear { get; set; }

        //----------------Notification Acknowledged -----------

        [Display(Name = "Day")]
        [Required]
        [Range(1, 31)]
        public int NotificationAcknowledgedDay { get; set; }

        [Display(Name = "Month")]
        [Required]
        [Range(1, 12)]
        public int NotificationAcknowledgedMonth { get; set; }

        [Display(Name = "Year")]
        [Required]
        [Range(0, 9999999)]
        public int NotificationAcknowledgedYear { get; set; }

        //----------------Decision Date-----------

        [Display(Name = "Day")]
        [Required]
        [Range(1, 31)]
        public int DecisionDay { get; set; }

        [Display(Name = "Month")]
        [Required]
        [Range(1, 12)]
        public int DecisionMonth { get; set; }

        [Display(Name = "Year")]
        [Required]
        [Range(0, 9999999)]
        public int DecisionYear { get; set; }
    }
}