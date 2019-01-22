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
    using Requests.Exporters;
    using Requests.Notification;
    using ViewModels.Exporter;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class ExporterController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMapWithParameter<ExporterViewModel, AddressRecordType, AddAddressBookEntry> addressBookMapper;
        private readonly IAuditService auditService;

        public ExporterController(IMediator mediator, IMapWithParameter<ExporterViewModel, AddressRecordType, AddAddressBookEntry> addressBookMapper, IAuditService auditService)
        {
            this.mediator = mediator;
            this.addressBookMapper = addressBookMapper;
            this.auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            ExporterViewModel model;
            var exporter = await mediator.SendAsync(new GetExporterByNotificationId(id));
            if (exporter.HasExporter)
            {
                model = new ExporterViewModel(exporter);
            }
            else
            {
                model = new ExporterViewModel
                {
                    NotificationId = id
                };
            }

            await this.BindCountryList(mediator);
            model.Address.DefaultCountryId = this.GetDefaultCountryId();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ExporterViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(mediator);
                return View(model);
            }

            try
            {
                var exporter = await mediator.SendAsync(new GetExporterByNotificationId(model.NotificationId));

                await mediator.SendAsync(model.ToRequest());

                await AddToProducerAddressBook(model);

                await this.auditService.AddAuditEntry(this.mediator,
                    model.NotificationId,
                    User.GetUserId(),
                    exporter.HasExporter ? NotificationAuditType.Updated : NotificationAuditType.Added,
                    NotificationAuditScreenType.Exporter);

                if (backToOverview.GetValueOrDefault())
                {
                    return RedirectToAction("Index", "Home", new { id = model.NotificationId });
                }
                else
                {
                    return RedirectToAction("List", "Producer", new { id = model.NotificationId });
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

            await this.BindCountryList(mediator);
            return View(model);
        }

        private async Task AddToProducerAddressBook(ExporterViewModel model)
        {
            if (!model.IsAddedToAddressBook)
            {
                return;
            }

            var addressRecord = addressBookMapper.Map(model, AddressRecordType.Producer);
                
            await mediator.SendAsync(addressRecord);
        }
    }
}