namespace EA.Iws.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Client.Entities;
    using Core.Cqrs;
    using Cqrs.Notification;
    using Domain;
    using CompetentAuthority = Client.Entities.CompetentAuthority;
    using WasteAction = Client.Entities.WasteAction;

    [Authorize]
    public class NotificationApplicationController : ApiController
    {
        private readonly ICommandBus commandBus;

        public NotificationApplicationController(ICommandBus commandBus)
        {
            this.commandBus = commandBus;
        }

        public async Task<IHttpActionResult> Post(CreateNotificationData notificationData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = CreateCommand(notificationData);
            await commandBus.SendAsync(command);

            return Ok(command.NotificationId);
        }

        private static CreateNotificationApplication CreateCommand(CreateNotificationData notificationData)
        {
            UKCompetentAuthority authority;
            Domain.Notification.WasteAction wasteAction;

            switch (notificationData.CompetentAuthority)
            {
                case CompetentAuthority.England:
                    authority = UKCompetentAuthority.England;
                    break;
                case CompetentAuthority.NorthernIreland:
                    authority = UKCompetentAuthority.NorthernIreland;
                    break;
                case CompetentAuthority.Scotland:
                    authority = UKCompetentAuthority.Scotland;
                    break;
                case CompetentAuthority.Wales:
                    authority = UKCompetentAuthority.Wales;
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown competent authority: {0}", notificationData.CompetentAuthority));
            }

            switch (notificationData.WasteAction)
            {
                case WasteAction.Recovery:
                    wasteAction = Domain.Notification.WasteAction.Recovery;
                    break;
                case WasteAction.Disposal:
                    wasteAction = Domain.Notification.WasteAction.Disposal;
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown waste action: {0}", notificationData.WasteAction));
            }

            return new CreateNotificationApplication
            {
                CompetentAuthority = authority,
                WasteAction = wasteAction
            };
        }
    }
}