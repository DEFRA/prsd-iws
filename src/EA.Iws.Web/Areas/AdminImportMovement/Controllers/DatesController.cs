namespace EA.Iws.Web.Areas.AdminImportMovement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;
    using ViewModels.Dates;

    [Authorize(Roles = "internal")]
    public class DatesController : Controller
    {
        private readonly IMediator mediator;

        public DatesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var data = await mediator.SendAsync(new GetImportMovementDates(id));

            return View(new DatesViewModel(data));
        } 
    }
}