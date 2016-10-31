namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.Transactions;
    using ViewModels.AccountManagement;
    using ViewModels.PaymentDetails;

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

            model.PaymentViewModel = new PaymentDetailsViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPayment(Guid id, PaymentDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var data = await mediator.SendAsync(new GetImportNotificationAccountOverview(id));
                var accountManagementViewModel = new AccountManagementViewModel(data);
                accountManagementViewModel.PaymentViewModel = model;
                accountManagementViewModel.ShowPaymentDetails = true;

                return View("Index", accountManagementViewModel);
            }

            await mediator.SendAsync(new AddNotificationPayment(id,
                model.PaymentAmount.Value,
                model.PaymentMethod,
                model.PaymentDate.AsDateTime().Value,
                model.ReceiptNumber,
                model.PaymentComments));

            return RedirectToAction("Index", "AccountManagement");
        }
    }
}