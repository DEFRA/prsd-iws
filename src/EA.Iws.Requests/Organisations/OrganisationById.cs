namespace EA.Iws.Requests.Organisations
{
    using System;
    using Core.Organisations;
    using Prsd.Core.Mediator;

    public class OrganisationById : IRequest<OrganisationData>
    {
        public readonly Guid Id;

        public OrganisationById(Guid id)
        {
            Id = id;
        }
    }
}