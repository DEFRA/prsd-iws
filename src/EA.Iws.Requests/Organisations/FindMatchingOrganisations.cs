namespace EA.Iws.Requests.Organisations
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Organisations;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadOrganisationData)]
    public class FindMatchingOrganisations : IRequest<IList<OrganisationData>>
    {
        public string CompanyName { get; private set; }

        public FindMatchingOrganisations(string companyName)
        {
            if (companyName == null)
            {
                throw new ArgumentNullException();
            }

            CompanyName = companyName;
        }
    }
}