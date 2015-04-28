namespace EA.Iws.Cqrs.Registration
{
    using System.Threading.Tasks;
    using Iws.Core.Cqrs;
    using Iws.DataAccess;

    internal class CreateOrganisationHandler : ICommandHandler<CreateOrganisation>
    {
        private readonly IwsContext db;

        public CreateOrganisationHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task HandleAsync(CreateOrganisation command)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                db.Organisations.Add(command.Organisation);

                try
                {
                    await db.SaveChangesAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }
    }
}