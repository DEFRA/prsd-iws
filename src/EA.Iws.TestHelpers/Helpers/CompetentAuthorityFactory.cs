namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using Domain;

    public class CompetentAuthorityFactory
    {
        public static CompetentAuthority Create(Guid id, Country country)
        {
            var competentAuthority = ObjectInstantiator<CompetentAuthority>.CreateNew();
            ObjectInstantiator<CompetentAuthority>.SetProperty(x => x.Country, country, competentAuthority);
            ObjectInstantiator<CompetentAuthority>.SetProperty(x => x.Id, id, competentAuthority);
            return competentAuthority;
        }
    }
}
