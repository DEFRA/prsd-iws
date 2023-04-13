namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.AddressBook;
    using Core.Notification.Audit;
    using DocumentFormat.OpenXml.Office2019.Word.Cid;
    using EA.Iws.Requests.NotificationAssessment;
    using EA.Iws.Web.Infrastructure.Authorization;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.AddressBook;
    using Requests.Exporters;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
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
        public ActionResult GetCompanyName(string registrationNumber)
        {
            if (!this.Request.IsAjaxRequest())
            {
                throw new InvalidOperationException();
            }

            //string companyHouseAPIHost = ConfigurationManager.AppSettings["Iws.CompanyHouseAPIHost"];
            //var url = "https://" + companyHouseAPIHost + "/DEFRA/v2.1/CompaniesHouse/companies/" + registrationNumber;
            //string filePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\Cert\\Boomi-IWS-TST.cer";

            //X509Certificate2 certificate = new X509Certificate2(filePath, "kN2S6!p6F*LH", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.ClientCertificates.Add(certificate);
            //request.Method = "GET";
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            var returnData = "A & B Test Company";
            return Json(returnData, JsonRequestBehavior.AllowGet);
        }
    }
}