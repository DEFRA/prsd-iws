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
    using Requests.Exporters;
    using ViewModels.Exporter;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class ExporterController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMapWithParameter<ExporterViewModel, AddressRecordType, AddAddressBookEntry> addressBookMapper;

        public ExporterController(IMediator mediator, IMapWithParameter<ExporterViewModel, AddressRecordType, AddAddressBookEntry> addressBookMapper)
        {
            this.mediator = mediator;
            this.addressBookMapper = addressBookMapper;
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
                await mediator.SendAsync(model.ToRequest());

                await AddToProducerAddressBook(model);

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