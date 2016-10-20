namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationMovements;
    using ViewModels.Cancel;

    [Authorize(Roles = "internal")]
    public class CancelController : Controller
    {
        private readonly IMediator mediator;

        public CancelController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET: AdminImportNotificationMovements/Cancel
        public async Task<ActionResult> Index(Guid id)
        {
            var result = await mediator.SendAsync(new GetCancellableMovements(id));

            var model = new CancellableMovementsViewModel(result);

            return View(model);
        }
    }
}