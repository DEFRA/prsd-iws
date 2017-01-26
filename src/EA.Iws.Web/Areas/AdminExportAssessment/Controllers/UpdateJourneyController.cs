namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using Requests.TransportRoute;
    using ViewModels.UpdateJourney;

    public class UpdateJourneyController : Controller
    {
        private readonly IMediator mediator;

        public UpdateJourneyController(IMediator mediator)
        {
            this.mediator = mediator;
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
                var stateOfImport = await mediator.SendAsync(new GetStateOfImportData(id));
                var entryPoints = await mediator.SendAsync(new GetEntryOrExitPointsByCountry(stateOfImport.Country.Id));

                model = new EntryPointViewModel(stateOfImport, entryPoints);

                return View(model);
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
        public ActionResult ExitPoint()
        {
            return View();
        }
    }
}