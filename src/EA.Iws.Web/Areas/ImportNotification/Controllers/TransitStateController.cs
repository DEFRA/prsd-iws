﻿namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.ImportNotification.TransportRoute;
    using Requests.Shared;
    using Requests.TransportRoute;
    using ViewModels.TransitState;

    [AuthorizeActivity(typeof(SetDraftData<>))]
    public class TransitStateController : Controller
    {
        private readonly IMediator mediator;

        public TransitStateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var transitStateCollection = await mediator.SendAsync(new GetDraftData<TransitStateCollection>(id));
            
            var model = new TransitStateCollectionViewModel
            {
                TransitStates = (await mediator.SendAsync(new GetTransitStateDataForTransitStates(transitStateCollection.TransitStates.ToList()))).ToList(),
                HasNoTransitStates = transitStateCollection.HasNoTransitStates
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, TransitStateCollectionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var transitStateCollection = await mediator.SendAsync(new GetDraftData<TransitStateCollection>(id));

            transitStateCollection.HasNoTransitStates = model.HasNoTransitStates;

            await mediator.SendAsync(new SetDraftData<TransitStateCollection>(id, transitStateCollection));

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            var model = new TransitStateViewModel
            {
                Countries = await mediator.SendAsync(new GetCountries())
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(Guid id, TransitStateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var transitStateCollection = await mediator.SendAsync(new GetDraftData<TransitStateCollection>(id));
            
            transitStateCollection.Add(model.AsTransitState(id));

            await mediator.SendAsync(new SetDraftData<TransitStateCollection>(id, transitStateCollection));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> GetCompetentAuthoritiesAndEntryOrExitPoints(Guid countryId)
        {
            var lookupData =
                await mediator.SendAsync(new GetTransitAuthoritiesAndEntryOrExitPointsByCountryId(countryId));

            var model = new TransitStateViewModel
            {
                CompetentAuthorities = lookupData.CompetentAuthorities,
                EntryOrExitPoints = lookupData.EntryOrExitPoints
            };

            return PartialView("_CompetentAuthoritiesAndEntryAndExitPoints", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id, Guid deleteId, TransitStateCollectionViewModel model)
        {
            var transitStateCollection = await mediator.SendAsync(new GetDraftData<TransitStateCollection>(id));

            transitStateCollection.Delete(deleteId);

            await mediator.SendAsync(new SetDraftData<TransitStateCollection>(id, transitStateCollection));

            model.TransitStates.Remove(model.TransitStates.SingleOrDefault(m => m.Id == deleteId));

            return PartialView("_Table", model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id, Guid entityId)
        {
            var transitStateCollection = await mediator.SendAsync(new GetDraftData<TransitStateCollection>(id));

            var transitState = transitStateCollection.TransitStates.Single(ts => ts.Id == entityId);

            var model = new TransitStateViewModel(transitState);
            model.Countries = await mediator.SendAsync(new GetCountries());

            if (model.IsCountrySelected)
            {
                var lookupData =
                    await
                        mediator.SendAsync(
                            new GetTransitAuthoritiesAndEntryOrExitPointsByCountryId(model.CountryId.Value));

                model.CompetentAuthorities = lookupData.CompetentAuthorities;
                model.EntryOrExitPoints = lookupData.EntryOrExitPoints;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, Guid entityId, TransitStateViewModel model)
        {
            var transitStateCollection = await mediator.SendAsync(new GetDraftData<TransitStateCollection>(id));

            var transitState = transitStateCollection.TransitStates.Single(ts => ts.Id == entityId);

            transitState.CountryId = model.CountryId;
            transitState.CompetentAuthorityId = model.CompetentAuthorityId;
            transitState.EntryPointId = model.EntryPointId;
            transitState.ExitPointId = model.ExitPointId;

            await mediator.SendAsync(new SetDraftData<TransitStateCollection>(id, transitStateCollection));

            return RedirectToAction("Index");
        }
    }
}