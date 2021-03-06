﻿namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Notification;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetSpecialHandingForNotificationHandler :
        IRequestHandler<GetSpecialHandingForNotification, SpecialHandlingData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, SpecialHandlingData> specialHandlingMapper;

        public GetSpecialHandingForNotificationHandler(IwsContext context, IMap<NotificationApplication, SpecialHandlingData> specialHandlingMapper)
        {
            this.context = context;
            this.specialHandlingMapper = specialHandlingMapper;
        }

        public async Task<SpecialHandlingData> HandleAsync(GetSpecialHandingForNotification message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            return specialHandlingMapper.Map(notification);
        }
    }
}