namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationMovements;
    using ViewModels.RejectedMovement;

    [Authorize(Roles = "internal")]
    public class RejectedMovementController : Controller
    {
        private readonly IMediator mediator;

        public RejectedMovementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> List(Guid id)
        {
            var model = new ListViewModel
            {
                Movements = await mediator.SendAsync(new GetRejectedImportMovements(id))
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult Details(Guid movementId)
        {
            return View();
        }
    }
}