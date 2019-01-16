namespace EA.Iws.RequestHandlers.Notification
{
    using Core.Notification.Audit;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class GetNotificationAuditScreensHandler : IRequestHandler<GetNotificationAuditScreens, IList<NotificationAuditScreen>>
    {
        private readonly IwsContext context;
        private readonly INotificationAuditScreenRepository repository;

        public GetNotificationAuditScreensHandler(IwsContext context, INotificationAuditScreenRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<IList<NotificationAuditScreen>> HandleAsync(GetNotificationAuditScreens message)
        {
            var screens = await this.repository.GetNotificationAuditScreens();

            return screens.Select(screen => this.Map(screen)).ToList();
        }

        private NotificationAuditScreen Map(AuditScreen screen)
        {
            return new NotificationAuditScreen()
            {
                Id = screen.Id,
                ScreenName = screen.ScreenName
            };
        }
    }
}
