namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using Core.Notification;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetWhatToDoNextDataForNotificationHandler : IRequestHandler<GetWhatToDoNextDataForNotification, WhatToDoNextData>
    {
        private readonly INotificationApplicationRepository repository;

        public GetWhatToDoNextDataForNotificationHandler(INotificationApplicationRepository repository)
        {
            this.repository = repository;
        }
        public async Task<WhatToDoNextData> HandleAsync(GetWhatToDoNextDataForNotification message)
        {
            var notification = await repository.GetById(message.Id);
            var data = new WhatToDoNextData
            {
                Id = message.Id,
                CompetentAuthority = (CompetentAuthority)notification.CompetentAuthority.Value
            };

            return data;
        }
    }
}
