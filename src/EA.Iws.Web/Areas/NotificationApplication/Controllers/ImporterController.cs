namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Importer;
    using ViewModels.Importer;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class ImporterController : Controller
    {
        private readonly IMediator mediator;

        public ImporterController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            ImporterViewModel model;
            var importer = await mediator.SendAsync(new GetImporterByNotificationId(id));
            if (importer.HasImporter)
            {
                model = new ImporterViewModel(importer);
            }
            else
            {
                model = new ImporterViewModel { NotificationId = id };
            }

            await this.BindCountryList(mediator, false);
            model.Address.DefaultCountryId = this.GetDefaultCountryId();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ImporterViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(mediator, false);
                return View(model);
            }

            try
            {
                await mediator.SendAsync(model.ToRequest());
                if (backToOverview.GetValueOrDefault())
                {
                    return RedirectToAction("Index", "Home", new { id = model.NotificationId });
                }
                else
                {
                    return RedirectToAction("List", "Facility", new { id = model.NotificationId });
                }
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);

                if (ModelState.IsValid)
                {
                    throw;
                }
            }
            await this.BindCountryList(mediator, false);
            return View(model);
        }
    }
}