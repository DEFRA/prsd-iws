namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.AddressBook;
    using Core.Notification.Audit;
    using EA.Iws.Api.Client.Entities;
    using EA.Iws.Web.Logging;
    using EA.IWS.Api.Infrastructure.Infrastructure;
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
        private readonly IElmahSqlLogger logger;

        public ExporterController(IMediator mediator, IMapWithParameter<ExporterViewModel, AddressRecordType, AddAddressBookEntry> addressBookMapper,
                                  IAuditService auditService, IElmahSqlLogger logger)
        {
            this.mediator = mediator;
            this.addressBookMapper = addressBookMapper;
            this.auditService = auditService;
            this.logger = logger;
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
        public async Task<ActionResult> GetCompanyNameAsync(string registrationNumber)
        {
            if (!this.Request.IsAjaxRequest())
            {
                throw new InvalidOperationException();
            }

            try
            {
                string orgName = DefraCompaniesHouseApi.GetOrganisationNameByRegNum(registrationNumber);
                return Json(new { success = true, message = orgName });
            }
            catch (WebException ex)
            {
                //Need to prepare error data
                ErrorData errorData = new ErrorData()
                {
                    Id = Guid.NewGuid(),
                    ApplicationName = "ExporterControllerDefraCompHouseCall",
                    Date = DateTime.Now,
                    ErrorXml = ErrorUtils.GetExceptionAsXml(ex, "DefraCompaniesHouseApi.GetOrganisationNameByRegNum(" + registrationNumber + ")"),
                    User = User.GetEmailAddress(),
                    Message = ex.Message,
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    StatusCode = 400,
                    HostName = string.Empty
                };

                ////Logging errors data into the database.
                await logger.Log(errorData);

                //HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                //return Json(new
                //{
                //    status = false,
                //    message = ex.Status.GetTypeCode()
                //}, JsonRequestBehavior.AllowGet);
                return Json(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                //Need to prepare error data
                ErrorData errorData = new ErrorData()
                {
                    Id = Guid.NewGuid(),
                    ApplicationName = "ExporterControllerDefraCompHouseCall",
                    Date = DateTime.Now,
                    ErrorXml = ErrorUtils.GetExceptionAsXml(ex, "DefraCompaniesHouseApi.GetOrganisationNameByRegNum(" + registrationNumber + ")"),
                    User = User.GetEmailAddress(),
                    Message = ex.Message,
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    StatusCode = 0,
                    HostName = string.Empty
                };

                //Logging errors data into the database.
                await logger.Log(errorData);

                //HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                //return Json(new { status = false, message = "Exception occured" }, JsonRequestBehavior.AllowGet);
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}