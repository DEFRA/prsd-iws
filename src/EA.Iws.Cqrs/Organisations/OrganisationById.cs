namespace EA.Iws.Cqrs.Organisations
{
    using System;
    using Core.Cqrs;
    using Domain;

    public class OrganisationById : IQuery<Organisation>
    {
        public readonly Guid Id;

        public OrganisationById(Guid id)
        {
            this.Id = id;
        }
    }
}
