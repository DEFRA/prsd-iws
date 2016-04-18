namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment;

    internal class ConsultationRepository : IConsultationRepository
    {
        private readonly ImportNotificationContext context;

        public ConsultationRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public void Add(Consultation consultation)
        {
            context.Consultations.Add(consultation);
        }
        
        public async Task<Consultation> GetByNotificationId(Guid notificationId)
        {
            return await context
                .Consultations
                .SingleOrDefaultAsync(c => c.NotificationId == notificationId);
        }
    }
}