namespace EA.Iws.RequestHandlers.NotificationMovements.Create
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.MeansOfTransport;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;

    internal class GetMeansOfTransportHandler : IRequestHandler<GetMeansOfTransport, IList<TransportMethod>>
    {
        private readonly IMeansOfTransportRepository repository;

        public GetMeansOfTransportHandler(IMeansOfTransportRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IList<TransportMethod>> HandleAsync(GetMeansOfTransport message)
        {
            var meansOfTransport = await repository.GetByNotificationId(message.NotificationId);

            return meansOfTransport.Route.ToList();
        }
    }
}