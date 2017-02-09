namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Files;
    using Requests.Movement;
    using Requests.Notification;

    [AuthorizeActivity(typeof(GetMovementFiles))]
    public class DocumentsController : Controller
    {
        private readonly IMediator mediator;

        public DocumentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, int page = 1)
        {
            var result = await mediator.SendAsync(new GetMovementFiles(id, page));
            var notification = await mediator.SendAsync(new GetNotificationBasicInfo(id));
            ViewBag.NotificationType = notification.NotificationType;

            return View(result);
        }

        [HttpGet]
        public async Task<ActionResult> Download(Guid id, Guid fileId)
        {
            var result = await mediator.SendAsync(new GetFile(id, fileId));

            return File(result.Content, MimeTypeHelper.GetMimeType(result.Type),
                string.Format("{0}.{1}", result.Name, result.Type));
        }
    }
}