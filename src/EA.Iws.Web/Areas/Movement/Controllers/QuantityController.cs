namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using ViewModels.Quantity;

    [Authorize]
    public class QuantityController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMap<MovementQuantityData, QuantityViewModel> quantityMap;

        public QuantityController(IMediator mediator, IMap<MovementQuantityData, QuantityViewModel> quantityMap)
        {
            this.mediator = mediator;
            this.quantityMap = quantityMap;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result = await mediator.SendAsync(new GetMovementQuantityDataByMovementId(id));

            return View(quantityMap.Map(result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, QuantityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new SetMovementQuantityByMovementId(id,
                Convert.ToDecimal(model.Quantity),
                model.Units.GetValueOrDefault()));

            return RedirectToAction("Index", "PackagingTypes", new { id });
        }
    }
}