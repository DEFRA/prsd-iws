namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.Movement.ViewModels;
    using Infrastructure;
    using Requests.MovementReceipt;

    public class DateReceivedController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public DateReceivedController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid movementId)
        {
            using (var client = apiClient())
            {
                var dateReceived = await client.SendAsync(User.GetAccessToken(), new GetMovementReceiptDateByMovementId(movementId));

                var viewModel = new DateReceivedViewModel(dateReceived);

                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid movementId, DateReceivedViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), new CreateMovementReceiptForMovement(movementId, viewModel.GetDateReceived()));

                return RedirectToAction("Index", "Home");
            }
        }
    }
}