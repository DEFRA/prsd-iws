namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using ViewModels.NumberOfPackages;

    public class NumberOfPackagesController : Controller
    {
        private readonly IMediator mediator;

        public NumberOfPackagesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result =
                await mediator.SendAsync(new GetNumberOfPackagesByMovementId(id));

            return View(new NumberOfPackagesViewModel { Number = result });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, NumberOfPackagesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new SetNumberOfPackagesByMovementId(id, model.Number.GetValueOrDefault()));

            return RedirectToAction("Index", "Carrier", new { id });
        }
    }
}