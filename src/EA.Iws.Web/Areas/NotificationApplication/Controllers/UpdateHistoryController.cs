namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using ViewModels.UpdateHistory;

    [Authorize]
    public class UpdateHistoryController : Controller
    {
        private readonly IMediator mediator;

        public UpdateHistoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var response = await mediator.SendAsync(new GetNotificationUpdateHistory(id));

            var model = new UpdateHistoryViewModel(response);
            //COULLM: Should this be set by the GetNotificationUpdateHistory constructor above?
            model.NotificationId = id;

            return View(model);
        }

        [HttpGet]
        public ActionResult NoChanges(Guid id)
        {
            var model = new NoChangesViewModel { NotificationId = id };

            return View(model);
        }
    }
}