namespace EA.Iws.Domain.NotificationApplication.Annexes
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;

    [AutoRegister]
    public class RequiredAnnexes
    {
        private readonly INotificationApplicationRepository notificationRepository;

        public RequiredAnnexes(INotificationApplicationRepository notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        public async Task<AnnexRequirements> Get(Guid notificationId)
        {
            var notification = await notificationRepository.GetById(notificationId);

            return
                new AnnexRequirements(
                    notification.HasTechnologyEmployed && notification.TechnologyEmployed.AnnexProvided, 
                    notification.WasteType != null && notification.WasteType.HasAnnex,
                    notification.IsWasteGenerationProcessAttached.GetValueOrDefault());
        } 
    }
}
