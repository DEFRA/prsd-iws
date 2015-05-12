namespace EA.Iws.Cqrs.Organisations
{
    using System;
    using Domain;
    using Prsd.Core.Mediator;

    public class OrganisationById : IRequest<Organisation>
    {
        public readonly Guid Id;

        public OrganisationById(Guid id)
        {
            Id = id;
        }
    }
}