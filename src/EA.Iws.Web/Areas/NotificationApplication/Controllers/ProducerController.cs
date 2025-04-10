﻿namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.Notification.Audit;
    using EA.Iws.Api.Client.CompaniesHouseAPI;
    using EA.Iws.Api.Client.Models;
    using EA.Iws.Web.Areas.Common;
    using EA.Iws.Web.Services;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.AddressBook;
    using Requests.Producers;
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Producer;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class ProducerController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMap<AddProducerViewModel, AddAddressBookEntry> producerAddressBookMap;
        private readonly IAuditService auditService;
        private readonly ITrimTextService trimTextService;
        private readonly Func<ICompaniesHouseClient> companiesHouseClient;
        private readonly ConfigurationService configurationService;

        public ProducerController(IMediator mediator, IMap<AddProducerViewModel, AddAddressBookEntry> producerAddressBookMap,
                                  IAuditService auditService, ITrimTextService trimTextService,
                                  Func<ICompaniesHouseClient> companiesHouseClient,
                                  ConfigurationService configurationService)
        {
            this.mediator = mediator;
            this.producerAddressBookMap = producerAddressBookMap;
            this.auditService = auditService;
            this.trimTextService = trimTextService;
            this.companiesHouseClient = companiesHouseClient;
            this.configurationService = configurationService;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id, bool? backToOverview = null)
        {
            var model = new AddProducerViewModel { NotificationId = id };

            if (model.Business?.Name?.Contains(" T/A ") == true)
            {
                string[] businessNames = model.Business.Name.Split(new[] { " T/A " }, 2, StringSplitOptions.None);
                model.Business.Name = businessNames[0];
                model.Business.OrgTradingName = businessNames[1];
            }

            await this.BindCountryList(mediator);
            model.Address.DefaultCountryId = this.GetDefaultCountryId();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AddProducerViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(mediator);
                return View(model);
            }

            try
            {
                if (!string.IsNullOrEmpty(model.Business?.OrgTradingName?.Trim()))
                {
                    model.Business.Name = model.Business.Name + " T/A " + model.Business.OrgTradingName;
                }

                //Trim address post code
                model.Address.PostalCode = trimTextService.RemoveTextWhiteSpaces(model.Address.PostalCode);

                var request = model.ToRequest();

                await mediator.SendAsync(request);

                await this.auditService.AddAuditEntry(this.mediator,
                    model.NotificationId,
                    User.GetUserId(),
                    NotificationAuditType.Added,
                    NotificationAuditScreenType.Producer);

                if (model.IsAddedToAddressBook)
                {
                    await mediator.SendAsync(producerAddressBookMap.Map(model));
                }

                return RedirectToAction("List", "Producer",
                    new { id = model.NotificationId, backToOverview });
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);

                if (ModelState.IsValid)
                {
                    throw;
                }
            }
            await this.BindCountryList(mediator);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id, Guid entityId, bool? backToOverview = null)
        {
            var producer =
                await mediator.SendAsync(new GetProducerForNotification(id, entityId));

            var model = new EditProducerViewModel(producer);

            if (model.Business?.Name?.Contains(" T/A ") == true)
            {
                string[] businessNames = model.Business.Name.Split(new[] { " T/A " }, 2, StringSplitOptions.None);
                model.Business.Name = businessNames[0];
                model.Business.OrgTradingName = businessNames[1];
            }

            await this.BindCountryList(mediator);
            model.Address.DefaultCountryId = this.GetDefaultCountryId();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditProducerViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(mediator);
                return View(model);
            }

            try
            {
                if (!string.IsNullOrEmpty(model.Business?.OrgTradingName?.Trim()))
                {
                    model.Business.Name = model.Business.Name + " T/A " + model.Business.OrgTradingName;
                }

                //Trim address post code
                model.Address.PostalCode = trimTextService.RemoveTextWhiteSpaces(model.Address.PostalCode);

                var request = model.ToRequest();

                await mediator.SendAsync(request);

                await this.auditService.AddAuditEntry(this.mediator,
                    model.NotificationId,
                    User.GetUserId(),
                    NotificationAuditType.Updated,
                    NotificationAuditScreenType.Producer);

                if (model.IsAddedToAddressBook)
                {
                    await mediator.SendAsync(producerAddressBookMap.Map(model));
                }

                return RedirectToAction("List", "Producer",
                    new { id = model.NotificationId, backToOverview });
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);

                if (ModelState.IsValid)
                {
                    throw;
                }
            }
            await this.BindCountryList(mediator);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Remove(Guid id, Guid entityId, bool? backToOverview = null)
        {
            ViewBag.BackToOverview = backToOverview.GetValueOrDefault();

            var response = await mediator.SendAsync(new GetProducersByNotificationId(id));
            var producer = response.Single(p => p.Id == entityId);

            var model = new RemoveProducerViewModel
            {
                NotificationId = id,
                ProducerId = entityId,
                ProducerName = producer.Business.Name,
                IsOnlySiteOfExport = producer.IsSiteOfExport && response.Count == 1 ? true : false
            };

            if (producer.IsSiteOfExport && response.Count > 1)
            {
                ViewBag.Error =
                    string.Format("You have chosen to remove {0} which is the site of export. " +
                                  "You will need to select an alternative site of export before you can remove this producer.",
                        model.ProducerName);
                return View(model);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Remove(RemoveProducerViewModel model, bool? backToOverview = null)
        {
            try
            {
                await mediator.SendAsync(new DeleteProducerForNotification(model.ProducerId, model.NotificationId));

                await this.auditService.AddAuditEntry(this.mediator,
                    model.NotificationId,
                    User.GetUserId(),
                    NotificationAuditType.Deleted,
                    NotificationAuditScreenType.Producer);

                if (model.IsOnlySiteOfExport)
                {
                    await this.auditService.AddAuditEntry(this.mediator,
                    model.NotificationId,
                    User.GetUserId(),
                    NotificationAuditType.Deleted,
                    NotificationAuditScreenType.SiteOfExport);
                }

                return RedirectToAction("List", "Producer", new { id = model.NotificationId, backToOverview });
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);
                if (ModelState.IsValid)
                {
                    throw;
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> List(Guid id, bool? backToOverview = null)
        {
            ViewBag.BackToOverview = backToOverview.GetValueOrDefault();

            var model = new MultipleProducersViewModel();
            var response =
                await mediator.SendAsync(new GetProducersByNotificationId(id));

            model.NotificationId = id;
            model.ProducerData = response;

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> SiteOfExport(Guid id, bool? backToList, bool? backToOverview = null)
        {
            ViewBag.BackToOverview = backToOverview.GetValueOrDefault();

            var response =
                await mediator.SendAsync(new GetProducersByNotificationId(id));

            var model = new SiteOfExportViewModel
            {
                NotificationId = id,
                Producers = response
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SiteOfExport(SiteOfExportViewModel model,
            bool? backToList,
            bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BackToOverview = backToOverview.GetValueOrDefault();

                var response = await mediator.SendAsync(new GetProducersByNotificationId(model.NotificationId));

                model.Producers = response;

                return View(model);
            }

            var existingProducers =
                await mediator.SendAsync(new GetProducersByNotificationId(model.NotificationId));

            await mediator.SendAsync(new SetSiteOfExport(
                model.SelectedSiteOfExport.GetValueOrDefault(),
                model.NotificationId));

            NotificationAuditType type = NotificationAuditType.Added;

            if (existingProducers != null && existingProducers.Count(p => p.IsSiteOfExport) > 0)
            {
                type = NotificationAuditType.Updated;
            }

            await this.auditService.AddAuditEntry(mediator,
                    model.NotificationId,
                    User.GetUserId(),
                    type,
                    NotificationAuditScreenType.SiteOfExport);

            if (backToList.GetValueOrDefault())
            {
                return RedirectToAction("List", "Producer", new { id = model.NotificationId, backToOverview });
            }

            if (backToOverview.GetValueOrDefault())
            {
                return RedirectToAction("Index", "Home", new { id = model.NotificationId });
            }

            return RedirectToAction("Index", "Importer", new { id = model.NotificationId });
        }

        [HttpGet]
        public async Task<ActionResult> GetCompanyName(string registrationNumber)
        {
            var result = await GetDefraCompanyDetails(registrationNumber);

            return Json(new { success = !result.Error, companyName = result.Organisation?.Name }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DefraCompaniesHouseApiModel> GetDefraCompanyDetails(string companyRegistrationNumber)
        {
            using (var client = companiesHouseClient())
            {
                return await client.GetCompanyDetailsAsync(configurationService.CurrentConfiguration.CompaniesHouseReferencePath, companyRegistrationNumber);
            }
        }
    }
}