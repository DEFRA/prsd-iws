namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.NotificationMovements;

    [Authorize(Roles = "internal")]
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
            var data = movementData.OrderBy(m => m.Number).Select(m => new DownloadMovementData(m)).ToList();

            var filename = string.Format("movement-details-for-notification-number-{0}.xlsx", notification.NotificationNumber);

            return new XlsxActionResult<DownloadMovementData>(data, filename);
        }
    }
}