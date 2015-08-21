namespace EA.Iws.Web.Areas.MovementDocument.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Movement;

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
        public async Task<ActionResult> Create(Guid id)
        {
            using (var client = apiClient())
            {
                var movementId = await client.SendAsync(User.GetAccessToken(), new CreateMovementForNotificationById(id));

                return RedirectToAction("Index", "ShipmentDate", new { id = movementId });
            }
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            return View();
        }
    }
}