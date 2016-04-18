namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using System.Threading.Tasks;
    using Domain;
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;
    using ImportDomain = Domain.ImportNotification;

    internal class AddressMap : IMap<ImportDomain.Address, Core.Address>
    {
        private readonly ICountryRepository countryRepository;

        public AddressMap(ICountryRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public Core.Address Map(ImportDomain.Address source)
        {
            return new Core.Address
            {
                AddressLine1 = source.Address1,
                AddressLine2 = source.Address2,
                TownOrCity = source.TownOrCity,
                Country = Task.Run(() => countryRepository.GetById(source.CountryId)).Result.Name,
                PostalCode = source.PostalCode
            };
        }
    }
}