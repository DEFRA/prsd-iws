namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core;
    using Prsd.Core.Extensions;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using Requests.Shared;
    using ViewModels.NotificationApplication;
    using Constants = Prsd.Core.Web.Constants;

    [Authorize]
    public class NotificationApplicationController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public NotificationApplicationController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> GenerateNotificationDocument(Guid id)
        {
            using (var client = apiClient())
            {
                try
                {
                    var response =
                        await client.SendAsync(User.GetAccessToken(), new GenerateNotificationDocument(id));

                    var downloadName = "IwsNotification" + SystemTime.UtcNow + ".docx";

                    return File(response, Constants.MicrosoftWordContentType, downloadName);
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

        [HttpGet]
        public async Task<ActionResult> NotificationOverview(Guid id)
        {
            using (var client = apiClient())
            {
                var response = await client.SendAsync(User.GetAccessToken(), new GetNotificationInfo(id));

                var model = new NotificationOverviewViewModel
                {
                    NotificationId = response.NotificationId,
                    NotificationNumber = response.NotificationNumber,
                    NotificationType = response.NotificationType                    
                };
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult ReasonForExport(Guid id)
        {
            var model = new ReasonForExportViewModel { NotificationId = id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReasonForExport(ReasonForExportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(), new SetReasonForExport(model.NotificationId, model.ReasonForExport));
                    return RedirectToAction("Add", "Carrier", new { id = model.NotificationId});
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);
                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
                return View(model);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult _Navigation(Guid id)
        {
            if (id == Guid.Empty)
            {
                return PartialView(new NotificationOverviewViewModel());
            }

            using (var client = apiClient())
            {
                var response = client.SendAsync(User.GetAccessToken(), new GetNotificationInfo(id)).GetAwaiter().GetResult();

                var model = new NotificationOverviewViewModel
                {
                    NotificationId = response.NotificationId,
                    NotificationNumber = response.NotificationNumber,
                    NotificationType = response.NotificationType
                };
                return PartialView(model);
            }
        }
    }
}