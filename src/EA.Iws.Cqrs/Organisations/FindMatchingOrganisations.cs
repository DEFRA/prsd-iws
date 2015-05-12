namespace EA.Iws.Cqrs.Organisations
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Prsd.Core.Mediator;

    public class FindMatchingOrganisations : IRequest<IList<OrganisationData>>
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