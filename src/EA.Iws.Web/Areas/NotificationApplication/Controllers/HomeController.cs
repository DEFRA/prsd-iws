namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Notification;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using ViewModels.NotificationApplication;

    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GenerateNotificationDocument(Guid id)
        {
            try
            {
                var response = await mediator.SendAsync(new GenerateNotificationDocument(id));

                return File(response.Content, MimeTypeHelper.GetMimeType(response.FileNameWithExtension),
                    response.FileNameWithExtension);
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

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var response = await mediator.SendAsync(new GetNotificationOverview(id));

            var model = new NotificationOverviewViewModel(response);

            ViewBag.Charge = response.NotificationCharge;

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult _Navigation(Guid id)
        {
            if (id == Guid.Empty)
            {
                return PartialView(new NotificationApplicationCompletionProgress());
            }

            var response = Task.Run(() => mediator.SendAsync(new GetNotificationProgressInfo(id))).Result;

            return PartialView(response);
        }
    }
}