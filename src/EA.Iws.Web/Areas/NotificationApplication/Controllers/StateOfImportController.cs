namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Requests.Shared;
    using Requests.StateOfImport;
    using Requests.TransportRoute;
    using ViewModels.StateOfImport;
    using Web.ViewModels.Shared;

    [Authorize]
    public class StateOfImportController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public StateOfImportController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            using (var client = apiClient())
            {
                await this.BindCountryList(client);
            }

            return View(new StateOfImportViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Add(Guid id, StateOfImportViewModel model, string selectCountry, string submit)
        {
            await this.BindCountryList(apiClient);

            if (!ModelState.IsValid && !string.IsNullOrWhiteSpace(submit))
            {
                await BindExitOrEntryPointSelectList(apiClient, model.CountryId);
                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(selectCountry))
            {
                return await StateOfImportCountrySelectedPostback(id, model);
            }

            try
            {
                using (var client = apiClient())
                {
                    await this.BindExitOrEntryPointSelectList(apiClient, model.CountryId);
                    await client.SendAsync(User.GetAccessToken(),
                        new AddStateOfImportToNotification(id, model.CountryId, (Guid)model.EntryOrExitPointId, model.CompetentAuthorities.SelectedValue));
                }
            }
            catch (ApiException)
            {
                ModelState.AddModelError(string.Empty, "Error saving this record.");
                return View(model);
            }

            return RedirectToAction("Summary", "TransportRoute", new { id });
        }

        private async Task<ActionResult> StateOfImportCountrySelectedPostback(Guid id, StateOfImportViewModel model)
        {
            this.RemoveModelStateErrors();
            using (var client = apiClient())
            {
                var competentAuthorities = await client.SendAsync(new GetCompetentAuthoritiesByCountry(model.CountryId));
                var radioButtons = competentAuthorities.Select(ca => new KeyValuePair<string, Guid>(string.Format("{0} - {1}", ca.Code, ca.Name), ca.Id));
                model.CompetentAuthorities = new StringGuidRadioButtons(radioButtons);
                model.LoadNextSection = true;

                await BindExitOrEntryPointSelectList(client, model.CountryId);
            }

            return View(model);
        }

        private async Task BindExitOrEntryPointSelectList(Func<IIwsClient> apiClient, Guid countryId)
        {
            using (var client = apiClient())
            {
                await BindExitOrEntryPointSelectList(client, countryId);
            }
        }

        private async Task BindExitOrEntryPointSelectList(IIwsClient client, Guid countryId)
        {
            var entryOrExitPoints = await client.SendAsync(new GetEntryOrExitPointsByCountry(countryId));
            ViewBag.EntryOrExitPoints = new SelectList(entryOrExitPoints, "Id", "Name");
        }
    }
}