namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shared;
    using Domain;
    using Prsd.Core.Mapper;
    using Requests.Shared;

    internal class BusinessMap : IMap<Business, BusinessData>
    {
        public BusinessData Map(Business source)
        {
            return new BusinessData
            {
                Name = source.Name,
                EntityType = source.Type,
                AdditionalRegistrationNumber = source.AdditionalRegistrationNumber,
                RegistrationNumber = source.RegistrationNumber
            };
        }
    }
}
