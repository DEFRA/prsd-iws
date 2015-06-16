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
    using Requests.TransitState;
    using Requests.TransportRoute;
    using ViewModels.Shared;
    using ViewModels.TransitState;

    [Authorize]
    public class TransitStateController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public TransitStateController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            using (var client = apiClient())
            {
                await this.BindCountryList(client, false);
            }

            return View(new TransitStateViewModel());
        } 

        [HttpPost]
        public async Task<ActionResult> Add(Guid id, TransitStateViewModel model, string selectCountry, string submit)
        {
            await this.BindCountryList(apiClient, false);

            if (!ModelState.IsValid)
            {
                if (model.CountryId == null)
                {
                    return View(model);
                }
                
                if (!string.IsNullOrWhiteSpace(submit))
                {
                    await BindExitOrEntryPointSelectList(apiClient, (Guid)model.CountryId);
                    return View(model);
                }
            }

            if (!string.IsNullOrWhiteSpace(selectCountry))
            {
                return await CountrySelectedPostback(id, model);
            }

            if (!model.CountryId.HasValue || !model.EntryPointId.HasValue || !model.ExitPointId.HasValue)
            {
                return View(model);
            }

            try
            {
                using (var client = apiClient())
                {
                    await this.BindExitOrEntryPointSelectList(apiClient, model.CountryId.Value);
                    await client.SendAsync(User.GetAccessToken(),
                       new AddTransitStateToNotification(id, 
                           model.CountryId.Value, 
                           model.EntryPointId.Value, 
                           model.ExitPointId.Value, 
                           model.CompetentAuthorities.SelectedValue));
                }
            }
            catch (ApiException)
            {
                ModelState.AddModelError(string.Empty, "Error saving this record.");
                return View(model);
            }

            return RedirectToAction("Summary", "TransportRoute", new { id });
        }

        private async Task<ActionResult> CountrySelectedPostback(Guid id, TransitStateViewModel model)
        {
            RemoveModelStateErrors();

            if (!model.CountryId.HasValue)
            {
                ModelState.AddModelError("CountryId", "Please select a country.");
                return View(model);
            }

            using (var client = apiClient())
            {
                var competentAuthorities = await client.SendAsync(new GetCompetentAuthoritiesByCountry(model.CountryId.Value));
                var radioButtons = competentAuthorities.Select(ca => new KeyValuePair<string, Guid>(string.Format("{0} - {1}", ca.Code, ca.Name), ca.Id));
                model.CompetentAuthorities = new StringGuidRadioButtons(radioButtons);
                model.LoadNextSection = true;

                await BindExitOrEntryPointSelectList(client, (Guid)model.CountryId);
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