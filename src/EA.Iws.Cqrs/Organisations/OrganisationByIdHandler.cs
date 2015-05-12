namespace EA.Iws.Cqrs.Organisations
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mediator;

    internal class OrganisationByIdHandler : IRequestHandler<OrganisationById, Organisation>
    {
        private readonly IwsContext context;

        public OrganisationByIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Organisation> HandleAsync(OrganisationById query)
        {
            return await context.Organisations.FindAsync(query.Id);
        }
    }
}