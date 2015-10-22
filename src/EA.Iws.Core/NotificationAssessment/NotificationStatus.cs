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
        [Display(Name = "Decision required")]
        DecisionRequiredBy = 7,
        [Display(Name = "Withdrawn")]
        Withdrawn = 8,
        [Display(Name = "Objected")]
        Objected = 9,
        [Display(Name = "Consented")]
        Consented = 10,
        [Display(Name = "Consent withdrawn")]
        ConsentWithdrawn = 11,
        [Display(Name = "In Determination")]
        InDetermination = 100
    }
}