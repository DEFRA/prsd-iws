namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;

    internal class ConsultationRepository : IConsultationRepository
    {
        private readonly IwsContext context;

        public ConsultationRepository(IwsContext context)
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