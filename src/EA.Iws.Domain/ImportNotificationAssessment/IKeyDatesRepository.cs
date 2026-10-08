namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using System.Threading.Tasks;

    public interface IKeyDatesRepository
    {
        Task SetNameOfOfficerForNotification(Guid notificationAssessmentId, string nameOfOfficer);
    }
}