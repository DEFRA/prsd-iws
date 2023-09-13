namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.AddressBook;
    using Core.Notification.Audit;
    using EA.Iws.Web.Areas.Common;
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
        private readonly ITrimTextService trimTextService;

        public ImporterController(IMediator mediator, IMapWithParameter<ImporterViewModel, AddressRecordType, AddAddressBookEntry> addressBookMapper, 
                                  IAuditService auditService, ITrimTextService trimTextService)
        {
            this.mediator = mediator;
            this.addressBookMapper = addressBookMapper;
            this.auditService = auditService;
            this.trimTextService = trimTextService;
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

            if (model.Business?.Name?.Contains(" T/A ") == true)
            {
                string[] businessNames = model.Business.Name.Split(new[] { " T/A " }, 2, StringSplitOptions.None);
                model.Business.Name = businessNames[0];
                model.Business.OrgTradingName = businessNames[1];
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

            if (!string.IsNullOrEmpty(model.Business?.OrgTradingName?.Trim()))
            {
                model.Business.Name = model.Business.Name + " T/A " + model.Business.OrgTradingName;
            }

            try
            {
                //Trim address post code
                model.Address.PostalCode = trimTextService.RemoveTextWhiteSpaces(model.Address.PostalCode);

                var importer = await mediator.SendAsync(new GetImporterByNotificationId(model.NotificationId));

                await mediator.SendAsync(model.ToRequest());

                await this.auditService.AddAuditEntry(this.mediator,
                   model.NotificationId,
                   User.GetUserId(),
                   importer.HasImporter ? NotificationAuditType.Updated : NotificationAuditType.Added,
                   NotificationAuditScreenType.Importer);

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