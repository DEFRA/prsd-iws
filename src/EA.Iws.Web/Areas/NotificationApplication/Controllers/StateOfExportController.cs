namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Requests.StateOfExport;
    using Requests.TransportRoute;
    using ViewModels.StateOfExport;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class StateOfExportController : Controller
    {
        private readonly Func<IIwsClient> apiClient;
        private readonly IMap<StateOfExportWithTransportRouteData, StateOfExportViewModel> mapper;

        public StateOfExportController(Func<IIwsClient> apiClient, IMap<StateOfExportWithTransportRouteData, StateOfExportViewModel> mapper)
        {
            this.apiClient = apiClient;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            using (var client = apiClient())
            {
                var stateOfExportSetData = await client.SendAsync(User.GetAccessToken(), new GetStateOfExportWithTransportRouteDataByNotificationId(id));

                var model = mapper.Map(stateOfExportSetData);

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, StateOfExportViewModel model, string submit, bool? backToOverview = null)
        {
            using (var client = apiClient())
            {
                await GetCompetentAuthoritiesAndEntryPoints(client, model);

                if (!ModelState.IsValid)         
                {
                    return View(model);
                }

                        return await SubmitAction(id, model, client, backToOverview);
            }
        }

        private async Task<ActionResult> SubmitAction(Guid id, StateOfExportViewModel model, IIwsClient client, bool? backToOverview)
        {
            await client.SendAsync(User.GetAccessToken(), new SetStateOfExportForNotification(id, model.EntryOrExitPointId.Value));

            if (backToOverview.GetValueOrDefault())
            {
                return RedirectToAction("Index", "Home", new { id });
            }
            else
            {
                return RedirectToAction("Index", "StateOfImport", new { id }); 
            }
        }

        private async Task GetCompetentAuthoritiesAndEntryPoints(IIwsClient client, StateOfExportViewModel model)
        {
            if (!model.CountryId.HasValue)
            {
                return;
            }

            var entryPointsAndCompetentAuthorities =
                await
                    client.SendAsync(User.GetAccessToken(), new GetCompetentAuthoritiesAndEntryOrExitPointsByCountryId(model.CountryId.Value));

            model.ExitPoints = new SelectList(entryPointsAndCompetentAuthorities.EntryOrExitPoints, "Id", "Name");
        }
    }
}