namespace EA.Iws.RequestHandlers.MeansOfTransport
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.MeansOfTransport;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.MeansOfTransport;

    internal class GetMeansOfTransportByNotificationIdHandler : IRequestHandler<GetMeansOfTransportByNotificationId, IList<TransportMethod>>
    {
        private readonly IMeansOfTransportRepository repository;

        public GetMeansOfTransportByNotificationIdHandler(IMeansOfTransportRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IList<TransportMethod>> HandleAsync(GetMeansOfTransportByNotificationId message)
        {
            var meansOfTransport = await repository.GetByNotificationId(message.Id);

            return meansOfTransport.Route.ToList();
        }
    }
}