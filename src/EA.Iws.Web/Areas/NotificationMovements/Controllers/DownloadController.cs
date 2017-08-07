namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Notification;
    using Requests.NotificationMovements;
    using ViewModels.Download;

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
            var data = movementData.Select(m => new DownloadMovementViewModel(m, notification.NotificationType, notification.NotificationNumber)).ToList();

            var filename = string.Format("movement-details-for-notification-number-{0}.csv", notification.NotificationNumber);

            return new CsvActionResult<DownloadMovementViewModel>(data, filename);
        }

        [HttpGet]
        public async Task<ActionResult> GenerateDocuments(Guid notificationId, Guid[] movementIds)
        {
            if (movementIds.Length == 1)
            {
                var result = await mediator.SendAsync(new GenerateMovementDocument(movementIds.Single()));

                return File(result.Content, MimeTypeHelper.GetMimeType(result.FileNameWithExtension),
                    result.FileNameWithExtension);
            }
            else
            {
                var result = await mediator.SendAsync(new GenerateMovementDocuments(movementIds));

                return File(result.Content, MimeTypeHelper.GetMimeType(result.FileNameWithExtension),
                    result.FileNameWithExtension);
            }
        }
    }
}