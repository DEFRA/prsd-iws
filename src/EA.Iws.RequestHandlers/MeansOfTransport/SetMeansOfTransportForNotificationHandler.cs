namespace EA.Iws.RequestHandlers.MeansOfTransport
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.MeansOfTransport;

    internal class SetMeansOfTransportForNotificationHandler : IRequestHandler<SetMeansOfTransportForNotification, Guid>
    {
        private readonly IMeansOfTransportRepository repository;
        private readonly IwsContext context;

        public SetMeansOfTransportForNotificationHandler(IwsContext context,
            IMeansOfTransportRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<Guid> HandleAsync(SetMeansOfTransportForNotification message)
        {
            var meansOfTransport = await repository.GetByNotificationId(message.Id);

            if (meansOfTransport == null)
            {
                meansOfTransport = new MeansOfTransport(message.Id);
            }

            meansOfTransport.SetRoute(message.MeansOfTransport);

            await context.SaveChangesAsync();

            return meansOfTransport.NotificationId;
        }
    }
}