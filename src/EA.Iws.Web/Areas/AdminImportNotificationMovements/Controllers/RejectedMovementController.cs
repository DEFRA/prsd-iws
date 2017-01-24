namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationMovements;
    using ViewModels.RejectedMovement;

    [AuthorizeActivity(typeof(GetRejectedImportMovements))]
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
        public async Task<ActionResult> Details(Guid movementId)
        {
            var movement = await mediator.SendAsync(new GetRejectedImportMovementDetails(movementId));
            var model = new DetailsViewModel(movement);

            return View(model);
        }
    }
}