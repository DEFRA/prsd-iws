namespace EA.Iws.Core.Shared
{
    using System.Collections.Generic;
    using Notification;

    public class CompetentAuthorityMetadata
    {
        private static IReadOnlyDictionary<CompetentAuthority, IEnumerable<string>> validEmailDomains
            = new Dictionary<CompetentAuthority, IEnumerable<string>>
            {
                { CompetentAuthority.England, new[] { "@environment-agency.gov.uk" } },
                { CompetentAuthority.Scotland, new[] { "@sepa.org.uk" } },
                { CompetentAuthority.NorthernIreland, new[] { "@doeni.gov.uk" } },
                { CompetentAuthority.Wales, new[] { "@cyfoethnaturiolcymru.gov.uk", "@naturalresourceswales.gov.uk" } }
            };

        public static IEnumerable<string> GetValidEmailAddressDomains(CompetentAuthority competentAuthority)
        {
            return validEmailDomains[competentAuthority];
        }
    }
}