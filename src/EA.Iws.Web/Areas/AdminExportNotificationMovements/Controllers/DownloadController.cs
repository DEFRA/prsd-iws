namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.NotificationMovements;

    [AuthorizeActivity(typeof(GetNotificationBasicInfo))]
    [AuthorizeActivity(typeof(GetMovementsByNotificationId))]
    public class DownloadController : Controller
    {
        private readonly IMediator mediator;

        public DownloadController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var notification = await mediator.SendAsync(new GetNotificationBasicInfo(id));
            var movementData = await mediator.SendAsync(new GetMovementsByNotificationId(id));
            var data = movementData.Select(m => new DownloadExportMovementData(m, notification.NotificationType)).ToList();

            var filename = string.Format("movement-details-for-notification-number-{0}.xlsx", notification.NotificationNumber);

            return new XlsxActionResult<DownloadExportMovementData>(data, filename);
        }
    }
}