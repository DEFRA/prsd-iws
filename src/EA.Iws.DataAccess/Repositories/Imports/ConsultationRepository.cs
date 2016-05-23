namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment;
    using Domain.Security;

    internal class ConsultationRepository : IConsultationRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ConsultationRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
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