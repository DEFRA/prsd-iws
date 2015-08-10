namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Shared;
    using Domain;
    using Prsd.Core.Mapper;

    internal class BacsDetailsMap : IMap<CompetentAuthorityBacsDetails, BacsData>
    {
        public BacsData Map(CompetentAuthorityBacsDetails source)
        {
            return new BacsData
            {
                AccountName = source.AccountName,
                AccountNumber = source.AccountNumber,
                Bank = source.Bank,
                BankAddress = source.BankAddress,
                Email = source.Email,
                Fax = source.Fax,
                Iban = source.Iban,
                SortCode = source.SortCode,
                SwiftBic = source.SwiftBic
            };
        }
    }
}
