namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.ImportNotificationMovements;
    using ViewModels.Download;

    [AuthorizeActivity(typeof(GetNotificationDetails))]
    [AuthorizeActivity(typeof(GetImportMovementsByNotificationId))]
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
            var notification = await mediator.SendAsync(new GetNotificationDetails(id));
            var movementData = await mediator.SendAsync(new GetImportMovementsByNotificationId(id));
            var data = movementData.Select(m => new DownloadMovementViewModel(m)).ToList();

            var filename = string.Format("movement-details-for-notification-number-{0}.xlsx", notification.NotificationNumber);

            return new XlsxActionResult<DownloadMovementViewModel>(data, filename);
        }
    }
}