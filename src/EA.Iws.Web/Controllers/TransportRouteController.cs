namespace EA.Iws.Web.Controllers
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
    using Requests.TransportRoute;
    using ViewModels.Shared;
    using ViewModels.TransportRoute;

    [Authorize]
    public class TransportRouteController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public TransportRouteController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> StateOfExport(Guid id)
        {
            using (var client = apiClient())
            {
                await this.BindCountryList(client);
            }

            return View(new StateOfExportViewModel
            {
                CountryId = Guid.Parse(((SelectList)ViewBag.Countries).SelectedValue.ToString())
            });
        }

        [HttpPost]
        public async Task<ActionResult> StateOfExport(Guid id, StateOfExportViewModel model, string selectCountry, string submit)
        {
            await this.BindCountryList(apiClient);

            if (!ModelState.IsValid && !string.IsNullOrWhiteSpace(submit))
            {
                await BindExitOrEntryPointSelectList(apiClient, model.CountryId);
                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(selectCountry))
            {
                return await StateOfExportCountrySelectedPostback(id, model);
            }

            try
            {
                using (var client = apiClient())
                {
                    await client.SendAsync(User.GetAccessToken(),
                        new AddStateOfExportToNotification(id, model.CountryId, (Guid)model.EntryOrExitPointId, model.CompetentAuthorities.SelectedValue));
                }
            }
            catch (ApiException)
            {
                ModelState.AddModelError(string.Empty, "Error saving this record.");
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        private async Task<ActionResult> StateOfExportCountrySelectedPostback(Guid id, StateOfExportViewModel model)
        {
            RemoveModelStateErrors();
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

        private void RemoveModelStateErrors()
        {
            foreach (var modelValue in ModelState.Values)
            {
                modelValue.Errors.Clear();
            }
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