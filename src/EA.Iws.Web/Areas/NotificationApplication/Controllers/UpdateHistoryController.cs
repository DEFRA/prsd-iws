namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using ViewModels.UpdateHistory;

    // Does this need an Authorize tag (like HomeController) or AuthorizeActivity tag (like OptionsController)?
    public class UpdateHistoryController : Controller
    {
        private readonly IMediator mediator;

        public UpdateHistoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var model = new UpdateHistoryViewModel { NotificationId = id };

            return View(model);
        }

        [HttpGet]
        public ActionResult NoChanges(Guid id)
        {
            var model = new UpdateHistoryViewModel { NotificationId = id };

            return View(model);
        }
    }
}