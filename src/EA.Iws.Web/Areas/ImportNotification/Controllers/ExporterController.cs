﻿namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using EA.Iws.Web.Areas.Common;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.Shared;
    using ViewModels.Exporter;

    [AuthorizeActivity(typeof(SetDraftData<>))]
    public class ExporterController : Controller
    {
        private readonly IMediator mediator;
        private readonly ITrimTextService trimTextService;

        public ExporterController(IMediator mediator, ITrimTextService trimTextService)
        {
            this.mediator = mediator;
            this.trimTextService = trimTextService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var exporter = await mediator.SendAsync(new GetDraftData<Exporter>(id));

            var countries = await mediator.SendAsync(new GetCountries());

            var model = new ExporterViewModel(exporter);

            model.Address.Countries = countries;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, ExporterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Trim address post code
            model.Address.PostalCode = trimTextService.RemoveTextWhiteSpaces(model.Address.PostalCode);

            var exporter = new Exporter(id)
            {
                Address = model.Address.AsAddress(),
                Type = model.BusinessType,
                BusinessName = (string.IsNullOrEmpty(model.Business.OrgTradingName) ? model.Business.Name : (model.Business.Name + " T/A " + model.Business.OrgTradingName)),
                RegistrationNumber = model.Business.RegistrationNumber,
                Contact = model.Contact.AsContact(),
                IsAddedToAddressBook = model.IsAddedToAddressBook
            };

            await mediator.SendAsync(new SetDraftData<Exporter>(id, exporter));

            return RedirectToAction("Index", "Importer");
        }
    }
}