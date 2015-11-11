namespace EA.Iws.RequestHandlers.NotificationMovements.Create
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.MeansOfTransport;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;

    internal class GetMeansOfTransportHandler : IRequestHandler<GetMeansOfTransport, IList<MeansOfTransport>>
    {
        private readonly INotificationApplicationRepository repository;

        public GetMeansOfTransportHandler(INotificationApplicationRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IList<MeansOfTransport>> HandleAsync(GetMeansOfTransport message)
        {
            var notification = await repository.GetById(message.NotificationId);

            return notification.MeansOfTransport.ToArray();
        }
    }
}