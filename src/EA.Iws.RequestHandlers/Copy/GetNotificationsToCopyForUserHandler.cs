namespace EA.Iws.RequestHandlers.Copy
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.WasteType;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Domain;
    using Prsd.Core.Helpers;
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
                .Join(context.Exporters, n => n.Id, e => e.NotificationId, (n, e) => new { Notification = n, Exporter = e })
                .Join(context.Importers, x => x.Notification.Id, i => i.NotificationId, (x, i) => new { Notification = x.Notification, Exporter = x.Exporter, Importer = i })
                .Where(x => x.Notification.UserId == userContext.UserId
                    && x.Notification.NotificationType == notificationType
                    && x.Notification.WasteType != null)
                .Select(x => new
                {
                    id = x.Notification.Id,
                    number = x.Notification.NotificationNumber,
                    exporter = x.Exporter.Business.Name,
                    importer = x.Importer.Business.Name,
                    waste = x.Notification.WasteType
                }).ToArrayAsync();

            return notificationData.Select(n => new NotificationApplicationCopyData
            {
                Id = n.id,
                Number = n.number,
                ExporterName = n.exporter,
                ImporterName = n.importer,
                WasteName = n.waste.ChemicalCompositionType == ChemicalComposition.Other ?
                n.waste.ChemicalCompositionName : EnumHelper.GetShortName(n.waste.ChemicalCompositionType)
            }).OrderBy(x => x.Number).ToArray();
        }
    }
}
