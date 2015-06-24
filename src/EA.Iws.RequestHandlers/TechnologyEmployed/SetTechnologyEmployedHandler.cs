namespace EA.Iws.RequestHandlers.TechnologyEmployed
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mediator;
    using Requests.TechnologyEmployed;

    public class SetTechnologyEmployedHandler : IRequestHandler<SetTechnologyEmployed, Guid>
    {
        private readonly IwsContext context;

        public SetTechnologyEmployedHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetTechnologyEmployed command)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);

            var technologyEmployed = command.AnnexProvided ? TechnologyEmployed.CreateTechnologyEmployedInAnnex() : TechnologyEmployed.CreateTechnologyEmployedDetails(command.Details);

            notification.SetTechnologyEmployed(technologyEmployed);

            await context.SaveChangesAsync();

            return notification.TechnologyEmployed.Id;
        }
    }
}