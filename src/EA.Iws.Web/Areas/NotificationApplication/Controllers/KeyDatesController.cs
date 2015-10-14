namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;
    using ViewModels.KeyDates;

    [Authorize]
    public class KeyDatesController : Controller
    {
        private readonly IMediator mediator;

        public KeyDatesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var dates = await mediator.SendAsync(new GetDates(id));
            var model = new KeyDatesViewModel(dates);
            model.NotificationId = id;

            return View(model);
        }
    }
}