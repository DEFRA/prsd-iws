namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.AddressBook;
    using Core.Notification.Audit;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.AddressBook;
    using Requests.Importer;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Importer;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class ImporterController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMapWithParameter<ImporterViewModel, AddressRecordType, AddAddressBookEntry> addressBookMapper;
        private readonly IAuditService auditService;

        public ImporterController(IMediator mediator, IMapWithParameter<ImporterViewModel, AddressRecordType, AddAddressBookEntry> addressBookMapper, IAuditService auditService)
        {
            this.mediator = mediator;
            this.addressBookMapper = addressBookMapper;
            this.auditService = auditService;
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
                var importer = await mediator.SendAsync(new GetImporterByNotificationId(model.NotificationId));

                await mediator.SendAsync(model.ToRequest());

                await this.auditService.AddAuditEntry(this.mediator,
                   model.NotificationId,
                   User.GetUserId(),
                   importer.HasImporter ? NotificationAuditType.Update : NotificationAuditType.Create,
                   "Importer");

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