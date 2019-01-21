namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using ViewModels.BulkUploadTemplate;

    [Authorize]
    public class BulkUploadTemplateController : Controller
    {
        private readonly IMediator mediator;

        public BulkUploadTemplateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            var data = await mediator.SendAsync(new GetNotificationBasicInfo(notificationId));
            var model = new BulkUploadTemplateViewModel()
            {
                NotificationId = data.NotificationId,
                NotificationNumber = data.NotificationNumber
            };
        
            return View(model);
        }

        [HttpGet]
        public ActionResult PrenotificationTemplate()
        {
            return View("Index");
        }
    }
}