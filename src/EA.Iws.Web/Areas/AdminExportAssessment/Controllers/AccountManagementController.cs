namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using ViewModels.AccountManagement;

    [Authorize(Roles = "internal")]
    public class AccountManagementController : Controller
    {
        private readonly IMediator mediator;

        public AccountManagementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var data = await mediator.SendAsync(new GetAccountManagementData(id));
            var model = new AccountManagementViewModel(data);

            ViewBag.ActiveSection = "Finance";
            return View(model);
        }
    }
}