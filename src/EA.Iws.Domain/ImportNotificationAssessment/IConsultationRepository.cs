namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using System.Threading.Tasks;

    public interface IConsultationRepository
    {
        Task<Consultation> GetByNotificationId(Guid notificationId);

        void Add(Consultation consultation);
    }
}