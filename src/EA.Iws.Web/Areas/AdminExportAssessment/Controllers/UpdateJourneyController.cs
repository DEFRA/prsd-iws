namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using Core.Notification.Audit;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Notification.AdditionalCharge;
    using EA.Iws.Core.Shared;
    using EA.Iws.Core.SystemSettings;
    using EA.Iws.Requests.AdditionalCharge;
    using EA.Iws.Requests.Notification;
    using EA.Iws.Requests.SystemSettings;
    using EA.Iws.Web.Infrastructure.AdditionalCharge;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using Requests.TransitState;
    using Requests.TransportRoute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.UpdateJourney;
    using Web.ViewModels.Shared;

    [AuthorizeActivity(typeof(SetEntryPoint))]
    [AuthorizeActivity(typeof(SetExitPoint))]
    [AuthorizeActivity(typeof(AddTransitState))]
    [AuthorizeActivity(typeof(RemoveTransitState))]
    [AuthorizeActivity(typeof(UpdateTransitStateEntryOrExit))]
    public class UpdateJourneyController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;
        private readonly IAdditionalChargeService additionalChargeService;

        public UpdateJourneyController(IMediator mediator, IAuditService auditService, IAdditionalChargeService additionalChargeService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
            this.additionalChargeService = additionalChargeService;
        }

        [HttpGet]
        public async Task<ActionResult> EntryPoint(Guid id)
        {
            var stateOfImport = await mediator.SendAsync(new GetStateOfImportData(id));
            var entryPoints = await mediator.SendAsync(new GetEntryOrExitPointsByCountry(stateOfImport.Country.Id));
            var competentAuthority = (await mediator.SendAsync(new GetNotificationBasicInfo(id))).CompetentAuthority;
            var notificationStatus = await mediator.SendAsync(new GetNotificationStatus(id));

            var model = new EntryPointViewModel(stateOfImport, entryPoints, id, competentAuthority, notificationStatus);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EntryPoint(Guid id, EntryPointViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await EntryPoint(id);
            }

            await mediator.SendAsync(new SetEntryPoint(id, model.SelectedEntryPoint.Value));

            await this.auditService.AddAuditEntry(this.mediator,
                    id,
                    User.GetUserId(),
                    NotificationAuditType.Updated,
                    NotificationAuditScreenType.ImportRoute);

            if (model.AdditionalCharge != null)
            {
                if (model.AdditionalCharge.IsAdditionalChargesRequired.HasValue && model.AdditionalCharge.IsAdditionalChargesRequired.Value)
                {
                    var addtionalCharge = CreateAdditionalChargeData(id, model.AdditionalCharge, AdditionalChargeType.UpdateEntryPoint);

                    await additionalChargeService.AddAdditionalCharge(mediator, addtionalCharge);
                }
            }

            return RedirectToAction("EntryPointChanged");
        }

        [HttpGet]
        public async Task<ActionResult> EntryPointChanged(Guid id)
        {
            var stateOfImport = await mediator.SendAsync(new GetStateOfImportData(id));

            ViewBag.EntryPoint = stateOfImport.EntryPoint.Name;

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ExitPoint(Guid id)
        {
            var stateOfExport = await mediator.SendAsync(new GetStateOfExportData(id));
            var entryPoints = await mediator.SendAsync(new GetEntryOrExitPointsByCountry(stateOfExport.Country.Id));
            var competentAuthority = (await mediator.SendAsync(new GetNotificationBasicInfo(id))).CompetentAuthority;
            var notificationStatus = await mediator.SendAsync(new GetNotificationStatus(id));

            var model = new ExitPointViewModel(stateOfExport, entryPoints, id, competentAuthority, notificationStatus);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExitPoint(Guid id, ExitPointViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await ExitPoint(id);
            }

            await mediator.SendAsync(new SetExitPoint(id, model.SelectedExitPoint.Value));

            await this.auditService.AddAuditEntry(this.mediator,
                    id,
                    User.GetUserId(),
                    NotificationAuditType.Updated,
                    NotificationAuditScreenType.ExportRoute);

            if (model.AdditionalCharge != null)
            {
                if (model.AdditionalCharge.IsAdditionalChargesRequired.HasValue && model.AdditionalCharge.IsAdditionalChargesRequired.Value)
                {
                    var addtionalCharge = CreateAdditionalChargeData(id, model.AdditionalCharge, AdditionalChargeType.UpdateExitPoint);

                    await additionalChargeService.AddAdditionalCharge(mediator, addtionalCharge);
                }
            }

            return RedirectToAction("ExitPointChanged");
        }

        [HttpGet]
        public async Task<ActionResult> ExitPointChanged(Guid id)
        {
            var stateOfImport = await mediator.SendAsync(new GetStateOfExportData(id));

            ViewBag.ExitPoint = stateOfImport.ExitPoint.Name;

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> AddTransitState(Guid id)
        {
            var countries = await mediator.SendAsync(new GetAllCountriesHavingCompetentAuthorities());

            var model = new AddTransitStateViewModel
            {
                Countries = new SelectList(countries, "Id", "Name")
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> GetTransitAuthoritiesAndEntryOrExitPointsByCountryId(Guid countryId)
        {
            var model = new AddTransitStateViewModel();

            var result = await mediator.SendAsync(new GetTransitAuthoritiesAndEntryOrExitPointsByCountryId(countryId));

            var competentAuthoritiesKeyValuePairs = result.CompetentAuthorities.Select(ca =>
                new KeyValuePair<string, Guid>(ca.Code + " - " + ca.Name, ca.Id));
            var competentAuthorityRadioButtons = new StringGuidRadioButtons(competentAuthoritiesKeyValuePairs);

            model.CompetentAuthorities = competentAuthorityRadioButtons;
            model.EntryOrExitPoints = new SelectList(result.EntryOrExitPoints, "Id", "Name");

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddTransitState(Guid id, AddTransitStateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var countries = await mediator.SendAsync(new GetAllCountriesHavingCompetentAuthorities());
                model.Countries = new SelectList(countries, "Id", "Name");

                var result = await mediator.SendAsync(new GetTransitAuthoritiesAndEntryOrExitPointsByCountryId(model.CountryId.Value));

                var competentAuthoritiesKeyValuePairs = result.CompetentAuthorities.Select(ca =>
                    new KeyValuePair<string, Guid>(ca.Code + " - " + ca.Name, ca.Id));
                var competentAuthorityRadioButtons = new StringGuidRadioButtons(competentAuthoritiesKeyValuePairs);

                model.CompetentAuthorities = competentAuthorityRadioButtons;
                model.EntryOrExitPoints = new SelectList(result.EntryOrExitPoints, "Id", "Name");

                return View(model);
            }

            await
                mediator.SendAsync(new AddTransitState(id, model.CountryId.Value, model.EntryPointId.Value,
                    model.ExitPointId.Value, model.CompetentAuthorities.SelectedValue));

            await this.auditService.AddAuditEntry(this.mediator,
                    id,
                    User.GetUserId(),
                    NotificationAuditType.Added,
                    NotificationAuditScreenType.Transits);

            return RedirectToAction("Index", "Overview");
        }

        [HttpGet]
        public async Task<ActionResult> RemoveTransitState(Guid id, Guid entityId)
        {
            var transitState =
                await mediator.SendAsync(new GetTransitStateWithTransportRouteDataByNotificationId(id, entityId));

            return View(transitState.TransitState);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("RemoveTransitState")]
        public async Task<ActionResult> RemoveTransitStatePost(Guid id, Guid entityId)
        {
            await mediator.SendAsync(new RemoveTransitState(id, entityId));

            await this.auditService.AddAuditEntry(this.mediator,
                    id,
                    User.GetUserId(),
                    NotificationAuditType.Deleted,
                    NotificationAuditScreenType.Transits);

            return RedirectToAction("Index", "Overview");
        }

        [HttpGet]
        public async Task<ActionResult> TransitEntryPoint(Guid id, Guid transitStateId)
        {
            var transitStateData =
                await mediator.SendAsync(new GetTransitStateWithEntryOrExitData(id, transitStateId));

            var model = new TransitEntryPointViewModel(transitStateData.TransitState, transitStateData.EntryOrExitPoints);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TransitEntryPoint(Guid id, Guid transitStateId, TransitEntryPointViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await TransitEntryPoint(id, transitStateId);
            }

            await mediator.SendAsync(new UpdateTransitStateEntryOrExit(id, transitStateId, model.SelectedEntryPoint, null));

            await auditService.AddAuditEntry(mediator,
                id,
                User.GetUserId(),
                NotificationAuditType.Updated,
                NotificationAuditScreenType.Transits);

            return RedirectToAction("TransitEntryPointChanged", new { id, transitStateId });
        }

        [HttpGet]
        public async Task<ActionResult> TransitEntryPointChanged(Guid id, Guid transitStateId)
        {
            var transitStateData =
                await mediator.SendAsync(new GetTransitStateWithEntryOrExitData(id, transitStateId));

            ViewBag.TransitEntryPoint = transitStateData.TransitState.EntryPoint.Name;

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> TransitExitPoint(Guid id, Guid transitStateId)
        {
            var transitStateData =
                await mediator.SendAsync(new GetTransitStateWithEntryOrExitData(id, transitStateId));

            var model = new TransitExitPointViewModel(transitStateData.TransitState, transitStateData.EntryOrExitPoints);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TransitExitPoint(Guid id, Guid transitStateId, TransitExitPointViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await TransitExitPoint(id, transitStateId);
            }

            await mediator.SendAsync(new UpdateTransitStateEntryOrExit(id, transitStateId, null, model.SelectedExitPoint));

            await auditService.AddAuditEntry(mediator,
                id,
                User.GetUserId(),
                NotificationAuditType.Updated,
                NotificationAuditScreenType.Transits);

            return RedirectToAction("TransitExitPointChanged", new { id, transitStateId });
        }

        [HttpGet]
        public async Task<ActionResult> TransitExitPointChanged(Guid id, Guid transitStateId)
        {
            var transitStateData =
                await mediator.SendAsync(new GetTransitStateWithEntryOrExitData(id, transitStateId));

            ViewBag.TransitExitPoint = transitStateData.TransitState.ExitPoint.Name;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetDefaultAdditionalChargeAmount(UKCompetentAuthority competentAuthority)
        {
            var response = new Core.SystemSetting.SystemSettingData();
            if (competentAuthority == UKCompetentAuthority.England)
            {
                response = await mediator.SendAsync(new GetSystemSettingById(SystemSettingType.EaAdditionalChargeFixedFee)); //EA
            }
            else if (competentAuthority == UKCompetentAuthority.Scotland)
            {
                response = await mediator.SendAsync(new GetSystemSettingById(SystemSettingType.SepaAdditionalChargeFixedFee)); //SEPA
            }

            return Json(response.Value);
        }

        private static CreateAdditionalCharge CreateAdditionalChargeData(Guid notificationId, AdditionalChargeData model, AdditionalChargeType additionalChargeType)
        {
            var createAddtionalCharge = new CreateAdditionalCharge()
            {
                ChangeDetailType = additionalChargeType,
                ChargeAmount = model.Amount,
                Comments = model.Comments,
                NotificationId = notificationId
            };

            return createAddtionalCharge;
        }
    }
}