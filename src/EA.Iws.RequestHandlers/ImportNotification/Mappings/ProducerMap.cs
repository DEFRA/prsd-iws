namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;
    using Domain = Domain.ImportNotification;

    internal class ProducerMap : IMap<Domain.Producer, Core.Producer>
    {
        private readonly IMapper mapper;

        public ProducerMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public Core.Producer Map(Domain.Producer source)
        {
            return new Core.Producer
            {
                AreMultiple = !source.IsOnlyProducer,
                Name = source.Name,
                Address = mapper.Map<Core.Address>(source.Address),
                Contact = mapper.Map<Core.Contact>(source.Contact)
            };
        }
    }
}