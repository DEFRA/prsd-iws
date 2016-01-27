namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Requests.TransitState;
    using Requests.TransportRoute;
    using ViewModels.TransitState;
    using Web.ViewModels.Shared;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class TransitStateController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMap<TransitStateWithTransportRouteData, TransitStateViewModel> transitStateMapper;

        private const string SelectCountry = "country";
        private const string ChangeCountry = "changeCountry";

        public TransitStateController(IMediator mediator, IMap<TransitStateWithTransportRouteData, TransitStateViewModel> transitStateMapper)
        {
            this.mediator = mediator;
            this.transitStateMapper = transitStateMapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, Guid? entityId, bool? backToOverview)
        {
            ViewBag.BackToOverview = backToOverview.GetValueOrDefault();

            TransitStateWithTransportRouteData result =
                    await
                        mediator.SendAsync(new GetTransitStateWithTransportRouteDataByNotificationId(id, entityId));

            var model = transitStateMapper.Map(result);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, Guid? entityId, TransitStateViewModel model, string submit, bool? backToOverview)
        {
            model.Countries = await GetCountrySelectListForModel(model);
            await GetCompetentAuthoritiesAndCountriesForModel(model);

            if (!ModelState.IsValid && submit != ChangeCountry)
            {
                return View(model);
            }

            switch (submit)
            {
                case SelectCountry:
                    return SelectCountryAction(model);
                case ChangeCountry:
                    return ChangeCountryAction(model);
                default:
                    return await SubmitAction(id, entityId, model, backToOverview);
            }
        }

        private ActionResult SelectCountryAction(TransitStateViewModel model)
        {
            model.ShowNextSection = true;

            return View("Index", model);
        }

        private ActionResult ChangeCountryAction(TransitStateViewModel model)
        {
            ModelState.Clear();
            model.ShowNextSection = false;

            return View("Index", model);
        }

        private async Task<ActionResult> SubmitAction(Guid id, Guid? transitStateId, TransitStateViewModel model, bool? backToOverview)
        {
            try
            {
                var request = new SetTransitStateForNotification(id,
                    model.CountryId.Value,
                    model.EntryPointId.Value,
                    model.ExitPointId.Value,
                    model.CompetentAuthorities.SelectedValue,
                    transitStateId,
                    model.OrdinalPosition);

                await mediator.SendAsync(request);

                return RedirectToAction("Summary", "TransportRoute", new { id, backToOverview });
            }
            catch (ApiException)
            {
                ModelState.AddModelError(string.Empty, "Error saving this record. You may already have saved this record, return to the summary to edit this record.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id, Guid delete, bool? backToOverview = null)
        {
            await mediator.SendAsync(new RemoveTransitStateForNotification(id, delete));

            return RedirectToAction("Summary", "TransportRoute", new { id, backToOverview });
        }

        private async Task GetCompetentAuthoritiesAndCountriesForModel(TransitStateViewModel model)
        {
            if (!model.CountryId.HasValue)
            {
                return;
            }

            var entryPointsAndCompetentAuthorities =
                await
                    mediator.SendAsync(new GetTransitAuthoritiesAndEntryOrExitPointsByCountryId(model.CountryId.Value));

            var competentAuthoritiesKeyValuePairs = entryPointsAndCompetentAuthorities.CompetentAuthorities.Select(ca =>
                new KeyValuePair<string, Guid>(ca.Code + " - " + ca.Name, ca.Id));
            var competentAuthorityRadioButtons = new StringGuidRadioButtons(competentAuthoritiesKeyValuePairs);

            if (model.CompetentAuthorities != null)
            {
                competentAuthorityRadioButtons.SelectedValue = model.CompetentAuthorities.SelectedValue;
            }

            model.CompetentAuthorities = competentAuthorityRadioButtons;
            model.EntryOrExitPoints = new SelectList(entryPointsAndCompetentAuthorities.EntryOrExitPoints, "Id", "Name");
        }

        private async Task<SelectList> GetCountrySelectListForModel(TransitStateViewModel model)
        {
            var countries = await mediator.SendAsync(new GetAllCountriesHavingCompetentAuthorities());

            return (model.CountryId.HasValue)
                ? new SelectList(countries, "Id", "Name", model.CountryId.Value)
                : new SelectList(countries, "Id", "Name");
        }
    }
}