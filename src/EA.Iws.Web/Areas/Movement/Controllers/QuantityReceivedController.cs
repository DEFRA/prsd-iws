namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.MovementReceipt;
    using ViewModels.Quantity;

    public class QuantityReceivedController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public QuantityReceivedController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            using (var client = apiClient())
            {
                var result = await client.SendAsync(User.GetAccessToken(), new GetMovementReceiptQuantityByMovementId(id));

                return View(new QuantityReceivedViewModel
                {
                    Unit = result.Unit,
                    Quantity = result.Quantity
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, QuantityReceivedViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), new SetMovementReceiptQuantityByMovementId(id,
                    model.Quantity.Value));

                return RedirectToAction("Index", "ReceiptComplete", new { id });
            }
        } 
    }
}