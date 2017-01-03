namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.NotificationMovements;

    [AuthorizeActivity(ExportMovementPermissions.CanReadExportMovements)]
    public class DownloadController : Controller
    {
        private readonly IMediator mediator;

        public DownloadController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            var notification = await mediator.SendAsync(new GetNotificationBasicInfo(notificationId));
            var movementData = await mediator.SendAsync(new GetMovementsByNotificationId(notificationId));
            var data = movementData.Select(m => new DownloadExportMovementData(m)).ToList();

            var filename = string.Format("movement-details-for-notification-number-{0}.csv", notification.NotificationNumber);

            return new CsvActionResult<DownloadExportMovementData>(data, filename);
        }
    }
}