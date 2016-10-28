namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
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
            var data = await mediator.SendAsync(new GetAccountManagementData(id));
            var model = new AccountManagementViewModel(data);

            model.PaymentViewModel = new PaymentDetailsViewModel{ NotificationId = id };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPayment(PaymentDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var data = await mediator.SendAsync(new GetAccountManagementData(model.NotificationId));
                var accountManagementViewModel = new AccountManagementViewModel(data);
                accountManagementViewModel.PaymentViewModel = model;
                accountManagementViewModel.ShowPaymentDetails = true;

                return View("Index", accountManagementViewModel);
            }

            if (model.PaymentMethod != PaymentMethod.Cheque)
            {
                model.Receipt = "NA";
            }

            var paymentData = new NotificationTransactionData
            {
                Date = model.Date.AsDateTime().Value,
                NotificationId = model.NotificationId,
                Credit = Convert.ToDecimal(model.Amount),
                PaymentMethod = model.PaymentMethod,
                ReceiptNumber = model.Receipt,
                Comments = model.Comments
            };

            await mediator.SendAsync(new AddNotificationTransaction(paymentData));

            return RedirectToAction("Index", "AccountManagement");
        }
    }
}