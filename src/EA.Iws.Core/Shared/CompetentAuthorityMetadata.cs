namespace EA.Iws.Core.Shared
{
    using System.Collections.Generic;
    using Notification;

    public class CompetentAuthorityMetadata
    {
        private static readonly IReadOnlyDictionary<UKCompetentAuthority, IEnumerable<string>> ValidEmailDomains
            = new Dictionary<UKCompetentAuthority, IEnumerable<string>>
            {
                { UKCompetentAuthority.England, new[] { "@environment-agency.gov.uk" } },
                { UKCompetentAuthority.Scotland, new[] { "@sepa.org.uk" } },
                { UKCompetentAuthority.NorthernIreland, new[] { "@daera-ni.gov.uk" } },
                { UKCompetentAuthority.Wales, new[] { "@cyfoethnaturiolcymru.gov.uk", "@naturalresourceswales.gov.uk" } }
            };

        public static IEnumerable<string> GetValidEmailAddressDomains(UKCompetentAuthority competentAuthority)
        {
            return ValidEmailDomains[competentAuthority];
        }
    }
}