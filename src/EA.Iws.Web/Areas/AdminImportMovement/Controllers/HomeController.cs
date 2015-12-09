namespace EA.Iws.Web.Areas.AdminImportMovement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;
    using ViewModels.Home;

    [Authorize(Roles = "internal")]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result = await mediator.SendAsync(new GetImportMovementReceiptAndRecoveryData(id));

            return View(new HomeViewModel(result));
        } 
    }
}