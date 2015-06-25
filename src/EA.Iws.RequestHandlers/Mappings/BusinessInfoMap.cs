namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using Core.Shared;
    using Domain;
    using Prsd.Core.Mapper;
    using Requests.Shared;
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
            if (type == Domain.BusinessType.LimitedCompany.DisplayName)
            {
                return BusinessType.LimitedCompany;
            }

            if (type == Domain.BusinessType.SoleTrader.DisplayName)
            {
                return BusinessType.SoleTrader;
            }

            if (type == Domain.BusinessType.Partnership.DisplayName)
            {
                return BusinessType.Partnership;
            }

            if (type == Domain.BusinessType.Other.DisplayName)
            {
                return BusinessType.Other;
            }

            throw new ArgumentException(string.Format("Unknown business type: {0}", type), "type");
        }
    }
}