namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.AddressBook;
    using Core.Notification.Audit;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;
    using Requests.NotificationAssessment;
    using ViewModels.Producer;

    [AuthorizeActivity(typeof(AddProducer))]
    public class ProducerController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public ProducerController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            var model = new AddProducerViewModel
            {
                NotificationId = id
            };

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
    }
}