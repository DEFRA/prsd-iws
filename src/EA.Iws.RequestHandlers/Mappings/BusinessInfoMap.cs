namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shared;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class BusinessInfoMap : IMap<Business, BusinessInfoData>
    {
        public BusinessInfoData Map(Business source)
        {
            return new BusinessInfoData
            {
                Name = source.Name,
                BusinessType = source.Type,
                AdditionalRegistrationNumber = source.AdditionalRegistrationNumber,
                RegistrationNumber = source.RegistrationNumber,
                OtherDescription = source.OtherDescription
            };
        }
    }
}