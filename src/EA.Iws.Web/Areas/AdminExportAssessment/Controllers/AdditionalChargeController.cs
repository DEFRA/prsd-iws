namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Requests.Notification;
    using EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.AdditionalCharge;
    using EA.Iws.Web.Infrastructure.Authorization;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    [AuthorizeActivity(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class AdditionalChargeController : Controller
    {
        private readonly IMediator mediator;

        public AdditionalChargeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var response = await mediator.SendAsync(new GetNotificationAdditionalCharges(id));

            var model = new AdditionalChargeViewModel()
            {
                AdditionalChargeData = response
            };

            return View(model);
        }
    }
}