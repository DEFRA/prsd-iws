namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Documents;
    using Infrastructure;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using Requests.NotificationMovements.BulkUpload;
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
        public async Task<ActionResult> PrenotificationTemplate()
        {
            try
            {
                var response = await mediator.SendAsync(new GetBulkUploadTemplate(BulkType.Prenotification));

                var downloadName = "BulkUploadPrenotificationTemplate" + SystemTime.UtcNow + ".xlsx";

                return File(response, MimeTypes.MSExcelXml, downloadName);
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);
                if (ModelState.IsValid)
                {
                    throw;
                }
                return HttpNotFound();
            }
        }
    }
}