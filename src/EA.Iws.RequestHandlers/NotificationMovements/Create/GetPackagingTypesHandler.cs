namespace EA.Iws.RequestHandlers.NotificationMovements.Create
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.PackagingType;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;

    internal class GetPackagingTypesHandler : IRequestHandler<GetPackagingTypes, PackagingData>
    {
        private readonly IMapper mapper;
        private readonly INotificationApplicationRepository repository;

        public GetPackagingTypesHandler(INotificationApplicationRepository repository,
            IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<PackagingData> HandleAsync(GetPackagingTypes message)
        {
            var notification = await repository.GetById(message.NotificationId);

            return mapper.Map<PackagingData>(notification.PackagingInfos
                .OrderBy(pi => pi.PackagingType)
                .ToList());
        }
    }
}