namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.MeansOfTransport;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.MeansOfTransport;
    using ViewModels.MeansOfTransport;

    [Authorize]
    public class MeansOfTransportController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public MeansOfTransportController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            using (var client = apiClient())
            {
                var model = new MeansOfTransportViewModel();

                var currentMeans =
                    await client.SendAsync(User.GetAccessToken(), new GetMeansOfTransportByNotificationId(id));

                if (currentMeans.Count != 0)
                {
                    model.SelectedMeans = string.Join("-", currentMeans.Select(p => p.Symbol));
                }

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, MeansOfTransportViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                using (var client = apiClient())
                {
                    var meansList = model.SelectedMeans.Split('-').Select(MeansOfTransport.GetFromToken).ToArray();

                    var result =
                        await
                            client.SendAsync(User.GetAccessToken(),
                                new SetMeansOfTransportForNotification(id, meansList));
                }
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);

                if (ModelState.IsValid)
                {
                    throw;
                }
            }

            if (backToOverview.GetValueOrDefault())
            {
                return RedirectToAction("Index", "Home", new { id });
            }
            else
            {
                return RedirectToAction("Index", "PackagingTypes", new { id }); 
            }
        }
    }
}