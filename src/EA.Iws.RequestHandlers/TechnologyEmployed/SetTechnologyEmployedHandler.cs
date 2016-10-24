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
        private readonly ITechnologyEmployedRepository technologyEmployedRepository;

        public SetTechnologyEmployedHandler(IwsContext context, ITechnologyEmployedRepository technologyEmployedRepository)
        {
            this.context = context;
            this.technologyEmployedRepository = technologyEmployedRepository;
        }

        public async Task<Guid> HandleAsync(SetTechnologyEmployed command)
        {
            var technologyEmployed = await technologyEmployedRepository.GetByNotificaitonId(command.NotificationId);

            if (technologyEmployed == null)
            {
                technologyEmployed = command.AnnexProvided
                    ? TechnologyEmployed.CreateTechnologyEmployedWithAnnex(command.NotificationId, command.Details)
                    : TechnologyEmployed.CreateTechnologyEmployedWithFurtherDetails(command.NotificationId,
                        command.Details, command.FurtherDetails);

                context.TechnologiesEmployed.Add(technologyEmployed);
            }
            else
            {
                if (command.AnnexProvided)
                {
                    technologyEmployed.SetWithAnnex(command.Details);
                }
                else
                {
                    technologyEmployed.SetWithFurtherDetails(command.Details, command.FurtherDetails);
                }
            }

            await context.SaveChangesAsync();

            return technologyEmployed.Id;
        }
    }
}