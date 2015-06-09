namespace EA.Iws.RequestHandlers.TechnologyEmployed
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.TechnologyEmployed;

    public class UpdateTechnologyEmployedHandler : IRequestHandler<UpdateTechnologyEmployed, Guid>
    {
        private readonly IwsContext context;

        public UpdateTechnologyEmployedHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(UpdateTechnologyEmployed command)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);
            notification.UpdateTechnologyEmployed(command.AnnexProvided, command.Details);

            await context.SaveChangesAsync();

            return notification.TechnologyEmployed.Id;
        }
    }
}
