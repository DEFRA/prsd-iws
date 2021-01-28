namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;    
    using Domain.NotificationApplication;
    using EA.Iws.Core.Notification;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationCompetentAuthorityHandler : IRequestHandler<GetNotificationCompetentAuthority, UKCompetentAuthority>
    {
        private readonly INotificationApplicationRepository repository;

        public GetNotificationCompetentAuthorityHandler(INotificationApplicationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<UKCompetentAuthority> HandleAsync(GetNotificationCompetentAuthority message)
        {
            return await repository.GetNotificationCompetentAuthority(message.NotificationId);
        }
    }
}
