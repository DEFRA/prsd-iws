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
    using Requests.Exporters;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Exporter;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class ExporterController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMapWithParameter<ExporterViewModel, AddressRecordType, AddAddressBookEntry> addressBookMapper;
        private readonly IAuditService auditService;
        private readonly ITrimTextMethod trimTextMethod;

        public ExporterController(IMediator mediator, IMapWithParameter<ExporterViewModel, AddressRecordType,
                                  AddAddressBookEntry> addressBookMapper, IAuditService auditService, ITrimTextMethod trimTextMethod)
        {
            this.mediator = mediator;
            this.addressBookMapper = addressBookMapper;
            this.auditService = auditService;
            this.trimTextMethod = trimTextMethod;
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

            if (model.Business?.Name?.Contains(" T/A ") == true)
            {
                string[] businessNames = model.Business.Name.Split(new[] { " T/A " }, 2, StringSplitOptions.None);
                model.Business.Name = businessNames[0];
                model.Business.OrgTradingName = businessNames[1];
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

                if (!string.IsNullOrEmpty(model.Business?.OrgTradingName?.Trim()))
                {
                    model.Business.Name = model.Business.Name + " T/A " + model.Business.OrgTradingName;
                }

                //Trim address post code
                model.Address.PostalCode = trimTextMethod.RemoveTextWhiteSpaces(model.Address.PostalCode);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCompanyName(string registrationNumber)
        {
            if (!this.Request.IsAjaxRequest())
            {
                throw new InvalidOperationException();
            }

            try
            {
                string orgName = DefraCompaniesHouseApi.GetOrganisationNameByRegNum(registrationNumber);
                return Json(new { success = true, companyName = orgName });
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    return Json(new { success = false, errorMsg = "Please enter valid company registration number and try again." });
                }
                else if (ex.Status == WebExceptionStatus.ConnectFailure)
                {
                    return Json(new { success = false, errorMsg = "Service is unavailable, please contatct system administator." });
                }
                else
                {
                    return Json(new { success = false, errorMsg = ex.Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMsg = ex.Message });
            }
        }
    }
}