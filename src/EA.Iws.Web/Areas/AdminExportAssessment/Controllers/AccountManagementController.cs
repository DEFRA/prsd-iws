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
    using ViewModels.RefundDetails;

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

            model.RefundViewModel = await GetNewRefundDetailsViewModel(id);

            return View(model);
        }

        private async Task<RefundDetailsViewModel> GetNewRefundDetailsViewModel(Guid id)
        {
            var limit = await mediator.SendAsync(new GetRefundLimit(id));

            var refundViewModel = new RefundDetailsViewModel
            {
                NotificationId = id,
                Limit = limit
            };

            return refundViewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPayment(PaymentDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var data = await mediator.SendAsync(new GetAccountManagementData(model.NotificationId));
                var accountManagementViewModel = new AccountManagementViewModel(data);
                accountManagementViewModel.RefundViewModel = await GetNewRefundDetailsViewModel(model.NotificationId);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRefund(RefundDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var data = await mediator.SendAsync(new GetAccountManagementData(model.NotificationId));
                var accountManagementViewModel = new AccountManagementViewModel(data);
                accountManagementViewModel.PaymentViewModel = new PaymentDetailsViewModel { NotificationId = model.NotificationId };
                accountManagementViewModel.RefundViewModel = model;
                accountManagementViewModel.ShowRefundDetails = true;

                return View("Index", accountManagementViewModel);
            }

            var refundData = new NotificationTransactionData
            {
                Date = model.Date.AsDateTime().Value,
                NotificationId = model.NotificationId,
                Debit = Convert.ToDecimal(model.Amount),
                Comments = model.Comments,
                ReceiptNumber = "NA"
            };

            await mediator.SendAsync(new AddNotificationTransaction(refundData));

            return RedirectToAction("index", "AccountManagement", new { id = model.NotificationId });
        }
    }
}