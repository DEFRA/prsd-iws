namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Threading.Tasks;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class GetCustomsCompletionStatusByNotificationIdHandler :
        IRequestHandler<GetCustomsCompletionStatusByNotificationId, CustomsOfficeCompletionStatus>
    {
        private readonly ITransportRouteRepository repository;

        public GetCustomsCompletionStatusByNotificationIdHandler(ITransportRouteRepository repository)
        {
            this.repository = repository;
        }

        public async Task<CustomsOfficeCompletionStatus> HandleAsync(GetCustomsCompletionStatusByNotificationId message)
        {
            var transportRoute = await repository.GetByNotificationId(message.Id);
            var requiredCustomsOffices = new RequiredCustomsOffices();

            return new CustomsOfficeCompletionStatus
            {
                CustomsOfficesRequired = requiredCustomsOffices.GetForTransportRoute(transportRoute)
            };
        }
    }
}