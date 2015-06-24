namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using ViewModels.NotificationApplication;

    public class ReasonForExportController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public ReasonForExportController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var model = new ReasonForExportViewModel { NotificationId = id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ReasonForExportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new SetReasonForExport(model.NotificationId, model.ReasonForExport));
                    return RedirectToAction("Add", "Carrier", new { id = model.NotificationId });
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
    }
}