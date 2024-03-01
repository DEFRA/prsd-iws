namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using Core.AddressBook;
    using Core.Notification.Audit;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Notification.AdditionalCharge;
    using EA.Iws.Core.Shared;
    using EA.Iws.Core.SystemSettings;
    using EA.Iws.Requests.AdditionalCharge;
    using EA.Iws.Requests.Notification;
    using EA.Iws.Requests.SystemSettings;
    using EA.Iws.Web.Infrastructure.AdditionalCharge;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;
    using Requests.NotificationAssessment;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Producer;

    [AuthorizeActivity(typeof(AddProducer))]
    public class ProducerController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;
        private readonly IAdditionalChargeService additionalChargeService;

        public ProducerController(IMediator mediator, IAuditService auditService, IAdditionalChargeService additionalChargeService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
            this.additionalChargeService = additionalChargeService;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            var competentAuthority = (await mediator.SendAsync(new GetNotificationBasicInfo(id))).CompetentAuthority;
            var notificationStatus = await mediator.SendAsync(new GetNotificationStatus(id));
            var model = new AddProducerViewModel(id, competentAuthority, notificationStatus);

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
        public async Task<ActionResult> Add(Guid id, AddProducerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(mediator);
                model.Address.DefaultCountryId = this.GetDefaultCountryId();

                return View(model);
            }

            if (!string.IsNullOrEmpty(model.Business?.OrgTradingName?.Trim()))
            {
                model.Business.Name = model.Business.Name + " T/A " + model.Business.OrgTradingName;
            }

            var request = new AddProducer
            {
                NotificationId = id,
                Address = model.Address,
                Business = model.Business.ToBusinessInfoData(),
                Contact = model.Contact
            };

            await mediator.SendAsync(request);

            await this.auditService.AddAuditEntry(this.mediator,
                    id,
                    User.GetUserId(),
                    NotificationAuditType.Added,
                    NotificationAuditScreenType.Producer);

            if (model.IsAddedToAddressBook)
            {
                await mediator.SendAsync(new AddAddressBookEntry
                {
                    Address = model.Address,
                    Business = model.Business.ToBusinessInfoData(),
                    Contact = model.Contact,
                    Type = AddressRecordType.Producer
                });
            }

            if (model.AdditionalCharge != null)
            {
                if (model.AdditionalCharge.IsAdditionalChargesRequired.HasValue && model.AdditionalCharge.IsAdditionalChargesRequired.Value)
                {
                    var addtionalCharge = new CreateAdditionalCharge(id, model.AdditionalCharge, AdditionalChargeType.AddProducer);

                    await additionalChargeService.AddAdditionalCharge(mediator, addtionalCharge);
                }
            }

            return RedirectToAction("Index", "Overview");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetDefaultAdditionalChargeAmount(UKCompetentAuthority competentAuthority)
        {
            var response = new Core.SystemSetting.SystemSettingData();
            if (competentAuthority == UKCompetentAuthority.England)
            {
                response = await mediator.SendAsync(new GetSystemSettingById(SystemSettingType.EaAdditionalChargeFixedFee)); //Id = 4 = EA
            }
            else if (competentAuthority == UKCompetentAuthority.Scotland)
            {
                response = await mediator.SendAsync(new GetSystemSettingById(SystemSettingType.SepaAdditionalChargeFixedFee)); //Id = 5 = SEPA
            }

            return Json(response.Value);
        }
    }
}