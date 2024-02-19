namespace EA.Iws.Core.Shared
{
    using EA.Iws.Core.Notification.AdditionalCharge;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AdditionalChargeData
    {
        public Guid NotificationId { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdditionalChargeDataResources), ErrorMessageResourceName = "IsAdditionalChargesRequiredErrorMessage")]
        [Display(Name = "IsAdditionalChargesRequired", ResourceType = typeof(AdditionalChargeDataResources))]
        public bool? IsAdditionalChargesRequired { get; set; }

        [Range(0, 1000)]
        [Display(Name = "AdditionalAmount", ResourceType = typeof(AdditionalChargeDataResources))]
        public decimal Amount { get; set; }

        [Display(Name = "Comments", ResourceType = typeof(AdditionalChargeDataResources))]
        public string Comments { get; set; }

        public AdditionalChargeType AdditionalChargeType { get; set; }
    }
}
