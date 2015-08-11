namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using EA.Iws.Core.Notification;
    using EA.Iws.Requests.Submit;
    using EA.Prsd.Core.Mapper;
    using Infrastructure;
    using Prsd.Core;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using ViewModels.NotificationApplication;
    using Constants = Prsd.Core.Web.Constants;

    [Authorize]
    public class HomeController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public HomeController(Func<IIwsClient> apiClient)
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
        public async Task<ActionResult> Index(Guid id)
        {
            using (var client = apiClient())
            {
                var response = await client.SendAsync(User.GetAccessToken(), new GetNotificationInfo(id));

                var model = new NotificationOverviewViewModel(response);

                ViewBag.Charge = response.NotificationCharge;

                return View(model);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult _Navigation(Guid id)
        {
            if (id == Guid.Empty)
            {
                return PartialView(new NotificationApplicationCompletionProgress());
            }

            using (var client = apiClient())
            {
                var response =
                    client.SendAsync(User.GetAccessToken(), new GetNotificationProgressInfo(id)).GetAwaiter().GetResult();

                return PartialView(response);
            }
        }

        [HttpGet]
        public ActionResult Disclaimer(Guid id)
        {
            var model = new DisclaimerViewModel { Id = id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disclaimer(DisclaimerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(),
                    new SubmitNotification(model.Id));
                return RedirectToAction("Index", "WhatToDoNext", new { id = model.Id });
            }
        }
    }
}