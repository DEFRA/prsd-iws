namespace EA.Iws.RequestHandlers.Copy
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Copy;

    internal class GetNotificationsToCopyForUserHandler : IRequestHandler<GetNotificationsToCopyForUser, IList<NotificationApplicationCopyData>>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public GetNotificationsToCopyForUserHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<IList<NotificationApplicationCopyData>> HandleAsync(GetNotificationsToCopyForUser message)
        {
            var notificationType =
                (await context.GetNotificationApplication(message.DestinationNotificationId)).NotificationType;

            var notificationData = await context.NotificationApplications
                .Join(context.Exporters, n => n.Id, e => e.NotificationId, (n, e) => new { n, e })
                .Where(na => na.n.UserId == userContext.UserId
                    && na.n.NotificationType == notificationType
                    && na.n.Importer != null
                    && na.n.WasteType != null)
                .Select(na => new
                {
                    id = na.n.Id,
                    number = na.n.NotificationNumber,
                    exporter = na.e.Business.Name,
                    importer = na.n.Importer.Business.Name,
                    waste = na.n.WasteType
                }).ToArrayAsync();

            return notificationData.Select(n => new NotificationApplicationCopyData
            {
                Id = n.id,
                Number = n.number,
                ExporterName = n.exporter,
                ImporterName = n.importer,
                WasteName = n.waste.ChemicalCompositionType == ChemicalComposition.Other ?
                n.waste.ChemicalCompositionName : n.waste.ChemicalCompositionType.DisplayName
            }).ToArray();
        }
    }
}
