namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shared;
    using Domain.NotificationApplication;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mapper;

    internal class BusinessMap : IMap<Business, BusinessData>
    {
        public BusinessData Map(Business source)
        {
            return new BusinessData
            {
                Name = source.Name,
                EntityType = EnumHelper.GetDisplayName(source.Type),
                AdditionalRegistrationNumber = source.AdditionalRegistrationNumber,
                RegistrationNumber = source.RegistrationNumber
            };
        }
    }
}