namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Movement;
    using ViewModels.NumberOfPackages;

    public class NumberOfPackagesController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public NumberOfPackagesController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            using (var client = apiClient())
            {
                var result =
                    await client.SendAsync(User.GetAccessToken(), new GetNumberOfPackagesByMovementId(id));

                return View(new NumberOfPackagesViewModel{ Number = result });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, NumberOfPackagesViewModel model)
        {
            using (var client = apiClient())
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await
                    client.SendAsync(User.GetAccessToken(),
                        new SetNumberOfPackagesByMovementId(id, model.Number.Value));

                return RedirectToAction("Index", "Carrier", new { id });
            }
        } 
    }
}