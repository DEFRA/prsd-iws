namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.AddressBook;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.AddressBook;
    using Requests.Importer;
    using ViewModels.Importer;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class ImporterController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMapWithParameter<ImporterViewModel, AddressRecordType, AddAddressBookEntry> addressBookMapper;

        public ImporterController(IMediator mediator, IMapWithParameter<ImporterViewModel, AddressRecordType, AddAddressBookEntry> addressBookMapper)
        {
            this.mediator = mediator;
            this.addressBookMapper = addressBookMapper;
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
                await AddToProducerAddressBook(model);

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

        private async Task AddToProducerAddressBook(ImporterViewModel model)
        {
            if (!model.IsAddedToAddressBook)
            {
                return;
            }

            var addressRecord = addressBookMapper.Map(model, AddressRecordType.Facility);

            await mediator.SendAsync(addressRecord);
        }
    }
}