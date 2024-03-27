namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using EA.Iws.Requests.ImportNotification;
    using EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.AdditionalCharge;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    [Authorize(Roles = "internal")]
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
            var response = await mediator.SendAsync(new GetImportNotificationAdditionalCharges(id));

            var model = new AdditionalChargeViewModel()
            {
                AdditionalChargeData = response
            };

            return View(model);
        }
    }
}