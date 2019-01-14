namespace EA.Iws.RequestHandlers.Mappings.NotificationAssessment
{
    using System.Threading.Tasks;
    using Core.Notification.Audit;
    using Domain;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class NotificationAuditMap : IMap<Audit, NotificationAuditForDisplay>
    {
        private readonly IInternalUserRepository internalUserRepository;
        private readonly INotificationAuditScreenRepository notificationAuditScreenRepository;
        private readonly INotificationUserRepository notificationUserRepository;
        
        public NotificationAuditMap(IInternalUserRepository internalUserRepository,
            INotificationAuditScreenRepository notificationAuditScreenRepository, INotificationUserRepository notificationUserRepository)
        {
            this.internalUserRepository = internalUserRepository;
            this.notificationAuditScreenRepository = notificationAuditScreenRepository;
            this.notificationUserRepository = notificationUserRepository;
        }

        public NotificationAuditForDisplay Map(Audit audit)
        {
            bool isInternalUser = Task.Run(() => internalUserRepository.IsUserInternal(audit.UserId)).Result;
            string userName = isInternalUser ? "Internal User" : Task.Run(() => notificationUserRepository.GetUserByUserId(audit.UserId)).Result.FullName;

            return new NotificationAuditForDisplay()
            {
                AuditType = ((NotificationAuditType)audit.Type).ToString(),
                DateAdded = audit.DateAdded,
                ScreenName = Task.Run(() => notificationAuditScreenRepository.GetNotificationAuditScreenById(audit.Screen)).Result.ScreenName,
                UserName = userName
            };
        }
    }
}
