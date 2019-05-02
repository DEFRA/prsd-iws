namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Update;
    using Infrastructure.Authorization;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.ImportNotificationAssessment;
    using Requests.TransportRoute;
    using ViewModels.UpdateJourney;

    [AuthorizeActivity(typeof(SetEntryPoint))]
    [AuthorizeActivity(typeof(SetExitPoint))]
    public class UpdateJourneyController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public UpdateJourneyController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> EntryPoint(Guid id)
        {
            var stateOfImport = await mediator.SendAsync(new GetStateOfImportData(id));
            var entryPoints = await mediator.SendAsync(new GetEntryOrExitPointsByCountry(stateOfImport.Country.Id));

            var model = new EntryPointViewModel(stateOfImport, entryPoints);

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

            var model = new ExitPointViewModel(stateOfExport, entryPoints);

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
                return View(model);
            }

            var wasteTypes = mapper.Map<WasteTypes>(model);

            await mediator.SendAsync(new UpdateImportNotificationWasteTypes(id, wasteTypes));

            return RedirectToAction("Index", "Home");
        }
    }
}