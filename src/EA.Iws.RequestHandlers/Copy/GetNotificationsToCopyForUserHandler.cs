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
                .Where(na => na.UserId == userContext.UserId
                    && na.NotificationType == notificationType
                    && na.Exporter != null 
                    && na.Importer != null
                    && na.WasteType != null)
                .Select(n => new
                {
                    id = n.Id,
                    number = n.NotificationNumber,
                    exporter = n.Exporter.Business.Name,
                    importer = n.Importer.Business.Name,
                    waste = n.WasteType
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
