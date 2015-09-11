namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Requests.Movement;
    using ViewModels.Quantity;

    [Authorize]
    public class QuantityController : Controller
    {
        private readonly Func<IIwsClient> apiClient;
        private readonly IMap<MovementQuantityData, QuantityViewModel> quantityMap;

        public QuantityController(Func<IIwsClient> apiClient, IMap<MovementQuantityData, QuantityViewModel> quantityMap)
        {
            this.apiClient = apiClient;
            this.quantityMap = quantityMap;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid movementId)
        {
            using (var client = apiClient())
            {
                var result = await client.SendAsync(User.GetAccessToken(), new GetMovementQuantityDataByMovementId(movementId));

                return View(quantityMap.Map(result));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid movementId, QuantityViewModel model)
        {
            using (var client = apiClient())
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await client.SendAsync(User.GetAccessToken(), new SetMovementQuantityByMovementId(movementId, 
                    Convert.ToDecimal(model.Quantity), 
                    model.Units.Value));

                return RedirectToAction("Index", "PackagingTypes", new { movementId });
            }
        }
    }
}