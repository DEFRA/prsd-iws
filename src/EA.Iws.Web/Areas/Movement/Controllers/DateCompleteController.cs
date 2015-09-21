namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.Movement.ViewModels;
    using Infrastructure;
    using Requests.MovementOperationReceipt;

    public class DateCompleteController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public DateCompleteController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            using (var client = apiClient())
            {
                var data = await client.SendAsync(User.GetAccessToken(), new GetMovementOperationReceiptDataByMovementId(id));

                var model = new DateCompleteViewModel(data.DateCompleted, data.NotificationType);

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, DateCompleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), new CreateMovementOperationReceiptForMovement(id, model.GetDateComplete()));

                return RedirectToAction("Index", "OperationComplete");
            }
        }
    }
}