namespace EA.Iws.Cqrs.Organisations
{
    using System;
    using System.Collections.Generic;
    using Core.Cqrs;
    using Domain;

    public class FindMatchingOrganisations : IQuery<IList<Organisation>>
    {
        public readonly string CompanyName;

        public FindMatchingOrganisations(string companyName)
        {
            if (companyName == null)
            {
                throw new ArgumentNullException();
            }

            this.CompanyName = companyName;
        }
    }
}
