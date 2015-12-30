namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;
    using ViewModels;
    using ViewModels.KeyDates;

    [Authorize(Roles = "internal")]
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
            var data = await mediator.SendAsync(new GetKeyDates(id));

            var model = new KeyDatesViewModel(data);

            return View(model);
        }
    }
}