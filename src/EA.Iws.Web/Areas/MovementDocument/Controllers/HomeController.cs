namespace EA.Iws.Web.Areas.MovementDocument.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Movement;
    using ViewModels;

    [Authorize]
    public class HomeController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public HomeController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Guid notificationId)
        {
            using (var client = apiClient())
            {
                var movementId = await client.SendAsync(User.GetAccessToken(), new CreateMovementForNotificationById(notificationId));

                return RedirectToAction("Index", "ShipmentDate", new { movementId });
            }
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            using (var client = apiClient())
            {
                var model = new MovementsViewModel
                {
                    Movements = await client.SendAsync(User.GetAccessToken(), new GetMovementsForNotificationById(notificationId))
                };

                return View(model);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Navigation(Guid notificationId, Guid movementId)
        {
            using (var client = apiClient())
            {
                var result =
                    client.SendAsync(User.GetAccessToken(),
                            new GetMovementProgressInformation(notificationId, movementId))
                            .GetAwaiter()
                            .GetResult();

                return PartialView("_Navigation", result);
            }
        }
    }
}