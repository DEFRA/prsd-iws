namespace EA.Iws.Domain.NotificationApplication.Annexes
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;

    [AutoRegister]
    public class RequiredAnnexes
    {
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly ITechnologyEmployedRepository technologyEmployedRepository;

        public RequiredAnnexes(INotificationApplicationRepository notificationRepository,
            ITechnologyEmployedRepository technologyEmployedRepository)
        {
            this.notificationRepository = notificationRepository;
            this.technologyEmployedRepository = technologyEmployedRepository;
        }

        public async Task<AnnexRequirements> Get(Guid notificationId)
        {
            var notification = await notificationRepository.GetById(notificationId);
            var technologyEmployed = await technologyEmployedRepository.GetByNotificaitonId(notificationId);

            return
                new AnnexRequirements(
                    technologyEmployed != null && technologyEmployed.AnnexProvided, 
                    notification.WasteType != null && notification.WasteType.HasAnnex,
                    notification.IsWasteGenerationProcessAttached.GetValueOrDefault());
        } 
    }
}
