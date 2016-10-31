namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.Transactions;
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
            var data = await mediator.SendAsync(new GetImportNotificationAccountOverview(id));
            var model = new AccountManagementViewModel(data);

            return View(model);
        } 
    }
}