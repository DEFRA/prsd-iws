namespace EA.Iws.Cqrs.Organisations
{
    using System.Threading.Tasks;
    using Core.Cqrs;
    using DataAccess;
    using Domain;

    internal class OrganisationByIdHandler : IQueryHandler<OrganisationById, Organisation>
    {
        private readonly IwsContext context;

        public OrganisationByIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Organisation> ExecuteAsync(OrganisationById query)
        {
            return await context.Organisations.FindAsync(query.Id);
        }
    }
}
