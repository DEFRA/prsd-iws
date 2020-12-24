namespace EA.Iws.Domain.TransportRoute
{
    using System.Collections.Generic;
    using System.Linq;

    public class TransportRouteValidation : ITransportRouteValidator
    {
        private readonly IEnumerable<IntraCountryExportAllowed> allowedCombinations;
        private readonly IEnumerable<UnitedKingdomCompetentAuthority> uksCompetentAuthorities;
        
        public TransportRouteValidation(IEnumerable<IntraCountryExportAllowed> allowedCombinations, IEnumerable<UnitedKingdomCompetentAuthority> unitedKingdomAuthorities)
        {
            this.allowedCombinations = allowedCombinations;
            this.uksCompetentAuthorities = unitedKingdomAuthorities;
        }

        public bool IsImportAndExportStatesCombinationValid(StateOfImport importState, StateOfExport exportState)
        {
            // Are both defined
            if (exportState == null || importState == null)
            {
                return true;
            }

            // are both same country?
            if (exportState.Country.Id != importState.Country.Id)
            {
                return true;
            }

            // Is UK authority? If not then we do not allow
            var unitedKingdomExportAuth = uksCompetentAuthorities.FirstOrDefault(uka => exportState.CompetentAuthority.Id == uka.CompetentAuthority.Id);
            if (unitedKingdomExportAuth == null)
            {
                return false;
            }

            // Now check if it is an allowed combination
            return allowedCombinations.Where(ac => ac.ExportCompetentAuthority == unitedKingdomExportAuth.AsCompetentAuthority())
                                      .Any(ac => ac.ImportCompetentAuthorityId == importState.CompetentAuthority.Id);
        }
    }
}
