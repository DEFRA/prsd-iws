namespace EA.Iws.Core.ImportNotificationAssessment
{
    using System.ComponentModel.DataAnnotations;

    public enum ImportNotificationStatus
    {
        New = 1,
        [Display(Name = "Awaiting payment")]
        NotificationReceived = 2
    }
}
