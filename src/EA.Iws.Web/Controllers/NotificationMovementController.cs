namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Movement;
    using ViewModels.Movement;

    [Authorize]
    public class NotificationMovementController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public NotificationMovementController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid? id)
        {
            using (var client = apiClient())
            {
                var model = new MovementsViewModel
                {
                    Movements = await client.SendAsync(User.GetAccessToken(), new GetMovementsForNotificationById(id.Value))
                };

                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Receipt(Guid id)
        {
            using (var client = apiClient())
            {
                var result =
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new GetActiveMovementsWithoutReceiptCertificateByNotificationId(id));

                return View(new MovementReceiptViewModel(id, result));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Receipt(Guid id, MovementReceiptViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Index", "DateReceived",
                new
                {
                    MovementId = model.RadioButtons.SelectedValue,
                    model.NotificationId,
                    area = "Movement"
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Guid id)
        {
            using (var client = apiClient())
            {
                var movementId = await client.SendAsync(User.GetAccessToken(), new CreateMovementForNotificationById(id));

                return RedirectToAction("Index", "ShipmentDate", new { movementId });
            }
        }
    }
}