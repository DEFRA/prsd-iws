namespace EA.Iws.RequestHandlers.TechnologyEmployed
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
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
            var notification = await context.GetNotificationApplication(command.NotificationId);

            var technologyEmployed = command.AnnexProvided
                ? TechnologyEmployed.CreateTechnologyEmployedWithAnnex(command.Details)
                : TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails(command.Details, command.FurtherDetails);

            notification.SetTechnologyEmployed(technologyEmployed);

            await context.SaveChangesAsync();

            return notification.TechnologyEmployed.Id;
        }
    }
}