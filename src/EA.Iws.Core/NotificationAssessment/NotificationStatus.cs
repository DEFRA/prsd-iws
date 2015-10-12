namespace EA.Iws.Core.NotificationAssessment
{
    using System.ComponentModel.DataAnnotations;

    public enum NotificationStatus
    {
        [Display(Name = "Not submitted")]
        NotSubmitted = 1,
        [Display(Name = "Submitted")]
        Submitted = 2,
        [Display(Name = "Notification received")]
        NotificationReceived = 3,
        [Display(Name = "In assessment")]
        InAssessment = 4,
        [Display(Name = "Ready to transmit")]
        ReadyToTransmit = 5,
        [Display(Name = "Transmitted")]
        Transmitted = 6,
        [Display(Name = "Acknowledged")]
        Acknowledged = 7,
        [Display(Name = "Decision required")]
        DecisionRequiredBy = 8,
        [Display(Name = "Withdrawn")]
        Withdrawn = 9,
        [Display(Name = "Objected")]
        Objected = 10,
        [Display(Name = "Consented")]
        Consented = 11,
        [Display(Name = "Consent withdrawn")]
        ConsentWithdrawn = 12,
        [Display(Name = "In Determination")]
        InDetermination = 100
    }
}