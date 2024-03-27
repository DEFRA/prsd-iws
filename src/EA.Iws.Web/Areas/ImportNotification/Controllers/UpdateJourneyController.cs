namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using Core.ImportNotification.Update;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Notification.AdditionalCharge;
    using EA.Iws.Core.SystemSettings;
    using EA.Iws.Requests.AdditionalCharge;
    using EA.Iws.Requests.SystemSettings;
    using EA.Iws.Web.Infrastructure.AdditionalCharge;
    using Infrastructure.Authorization;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.ImportNotificationAssessment;
    using Requests.TransportRoute;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.UpdateJourney;

    [AuthorizeActivity(typeof(SetEntryPoint))]
    [AuthorizeActivity(typeof(SetExitPoint))]
    [AuthorizeActivity(typeof(UpdateImportNotificationWasteTypes))]
    [AuthorizeActivity(typeof(UpdateWasteOperation))]
    public class UpdateJourneyController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IAdditionalChargeService additionalChargeService;

        public UpdateJourneyController(IMediator mediator, IMapper mapper, IAdditionalChargeService additionalChargeService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.additionalChargeService = additionalChargeService;
        }

        [HttpGet]
        public async Task<ActionResult> EntryPoint(Guid id)
        {
            var stateOfImport = await mediator.SendAsync(new GetStateOfImportData(id));
            var entryPoints = await mediator.SendAsync(new GetEntryOrExitPointsByCountry(stateOfImport.Country.Id));

            var importNotificationDetails = await mediator.SendAsync(new GetNotificationDetails(id));

            var model = new EntryPointViewModel(stateOfImport, entryPoints,
                importNotificationDetails.ImportNotificationId, importNotificationDetails.CompetentAuthority, importNotificationDetails.Status);

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

            if (model.AdditionalCharge != null)
            {
                if (model.AdditionalCharge.IsAdditionalChargesRequired.HasValue && model.AdditionalCharge.IsAdditionalChargesRequired.Value)
                {
                    var addtionalCharge = new CreateImportNotificationAdditionalCharge(id, model.AdditionalCharge, AdditionalChargeType.UpdateEntryPoint);

                    await additionalChargeService.AddImportAdditionalCharge(mediator, addtionalCharge);
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

            var importNotificationDetails = await mediator.SendAsync(new GetNotificationDetails(id));

            var model = new ExitPointViewModel(stateOfExport, entryPoints,
                importNotificationDetails.ImportNotificationId, importNotificationDetails.CompetentAuthority, importNotificationDetails.Status);

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

            if (model.AdditionalCharge != null)
            {
                if (model.AdditionalCharge.IsAdditionalChargesRequired.HasValue && model.AdditionalCharge.IsAdditionalChargesRequired.Value)
                {
                    var addtionalCharge = new CreateImportNotificationAdditionalCharge(id, model.AdditionalCharge, AdditionalChargeType.UpdateExitPoint);

                    await additionalChargeService.AddImportAdditionalCharge(mediator, addtionalCharge);
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
        public async Task<ActionResult> WasteCodes(Guid id)
        {
            var data = await mediator.SendAsync(new GetImportNotificationWasteTypes(id));

            var model = mapper.Map<UpdateWasteCodesViewModel>(data);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WasteCodes(Guid id, UpdateWasteCodesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var data = await mediator.SendAsync(new GetImportNotificationWasteTypes(id));

                model = mapper.Map<UpdateWasteCodesViewModel>(model, data.AllCodes);

                return View(model);
            }

            var wasteTypes = mapper.Map<WasteTypes>(model);

            await mediator.SendAsync(new UpdateImportNotificationWasteTypes(id, wasteTypes));

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> WasteOperation(Guid id)
        {
            var data = await mediator.SendAsync(new GetWasteOperationData(id));

            return View(new UpdateWasteOperationViewModel(data));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WasteOperation(Guid id, UpdateWasteOperationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var details = await mediator.SendAsync(new GetNotificationDetails(id));

                model.SetDetails(details);

                return View(model);
            }

            await mediator.SendAsync(new UpdateWasteOperation(id, model.SelectedCodes, model.TechnologyEmployed));

            return RedirectToAction("Index", "Home");
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
    }
}