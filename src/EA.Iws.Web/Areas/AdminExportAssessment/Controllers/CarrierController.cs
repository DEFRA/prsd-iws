namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Notification.Audit;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Carriers;
    using ViewModels.Carrier;

    [AuthorizeActivity(typeof(AddCarrierToNotification))]
    public class CarrierController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public CarrierController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            var model = new AddCarrierViewModel { NotificationId = id };
            await this.BindCountryList(mediator);
            model.Address.DefaultCountryId = this.GetDefaultCountryId();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(NotificationApplication.ViewModels.Carrier.AddCarrierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(mediator);
                return View(model);
            }
            try
            {
                await mediator.SendAsync(model.ToRequest());

                await auditService.AddAuditEntry(mediator,
                    model.NotificationId,
                    User.GetUserId(),
                    NotificationAuditType.Added,
                    NotificationAuditScreenType.IntendedCarrier);

                return RedirectToAction("Index", "Overview");
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
    }
}