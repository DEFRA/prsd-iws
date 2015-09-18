namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shared;
    using Domain;
    using Prsd.Core.Mapper;

    internal class BusinessMap : IMap<Business, BusinessData>
    {
        public BusinessData Map(Business source)
        {
            return new BusinessData
            {
                Name = source.Name,
                EntityType = source.Type.DisplayName,
                AdditionalRegistrationNumber = source.AdditionalRegistrationNumber,
                RegistrationNumber = source.RegistrationNumber
            };
        }
    }
}
