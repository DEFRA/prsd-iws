namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using Core.Shared;
    using Domain;
    using Prsd.Core.Mapper;
    using BusinessType = Core.Shared.BusinessType;

    internal class BusinessInfoMap : IMap<Business, BusinessInfoData>
    {
        public BusinessInfoData Map(Business source)
        {
            return new BusinessInfoData
            {
                Name = source.Name,
                BusinessType = GetBusinessType(source.Type),
                AdditionalRegistrationNumber = source.AdditionalRegistrationNumber,
                RegistrationNumber = source.RegistrationNumber,
                OtherDescription = source.OtherDescription
            };
        }

        private BusinessType GetBusinessType(string type)
        {
            if (type.Equals(Domain.BusinessType.LimitedCompany.DisplayName, StringComparison.OrdinalIgnoreCase))
            {
                return BusinessType.LimitedCompany;
            }

            if (type.Equals(Domain.BusinessType.SoleTrader.DisplayName, StringComparison.OrdinalIgnoreCase))
            {
                return BusinessType.SoleTrader;
            }

            if (type.Equals(Domain.BusinessType.Partnership.DisplayName, StringComparison.OrdinalIgnoreCase))
            {
                return BusinessType.Partnership;
            }

            if (type.Equals(Domain.BusinessType.Other.DisplayName, StringComparison.OrdinalIgnoreCase))
            {
                return BusinessType.Other;
            }

            throw new ArgumentException(string.Format("Unknown business type: {0}", type), "type");
        }
    }
}