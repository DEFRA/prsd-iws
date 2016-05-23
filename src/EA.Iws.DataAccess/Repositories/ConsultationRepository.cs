namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using Domain.Security;

    internal class ConsultationRepository : IConsultationRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization authorization;

        public ConsultationRepository(IwsContext context, INotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public void Add(Consultation consultation)
        {
            context.Consultations.Add(consultation);
        }

        public async Task<Consultation> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);

            return await context
                .Consultations
                .SingleOrDefaultAsync(c => c.NotificationId == notificationId);
        }
    }
}