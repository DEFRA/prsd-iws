namespace EA.Iws.RequestHandlers.TechnologyEmployed
{
    using System.Threading.Tasks;
    using Core.TechnologyEmployed;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.TechnologyEmployed;

    internal class GetTechnologyEmployedHandler : IRequestHandler<GetTechnologyEmployed, TechnologyEmployedData>
    {
        private readonly ITechnologyEmployedRepository technologyEmployedRepository;
        private readonly IMap<TechnologyEmployed, TechnologyEmployedData> mapper;

        public GetTechnologyEmployedHandler(ITechnologyEmployedRepository technologyEmployedRepository,
            IMap<TechnologyEmployed, TechnologyEmployedData> mapper)
        {
            this.technologyEmployedRepository = technologyEmployedRepository;
            this.mapper = mapper;
        }

        public async Task<TechnologyEmployedData> HandleAsync(GetTechnologyEmployed message)
        {
            var technologyEmployed = await technologyEmployedRepository.GetByNotificaitonId(message.NotificationId);

            return mapper.Map(technologyEmployed);
        }
    }
}