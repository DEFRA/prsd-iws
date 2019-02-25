namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.NotificationAssessment;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;
    using Requests.ImportNotificationAssessment.Transactions;
    using ViewModels.AccountManagement;
    using ViewModels.PaymentDetails;
    using ViewModels.RefundDetails;

    [AuthorizeActivity(typeof(GetImportNotificationAccountOverview))]
    public class AccountManagementController : Controller
    {
        private readonly IMediator mediator;
        private readonly AuthorizationService authorizationService;

        public AccountManagementController(IMediator mediator, AuthorizationService authorizationService)
        {
            this.mediator = mediator;
            this.authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var data = await mediator.SendAsync(new GetImportNotificationAccountOverview(id));
            var accountManagementViewModel = new AccountManagementViewModel(data);
            var canDeleteTransaction = await authorizationService.AuthorizeActivity(typeof(DeleteTransaction));

            accountManagementViewModel.PaymentViewModel = new PaymentDetailsViewModel();
            accountManagementViewModel.RefundViewModel = await GetNewRefundDetailsViewModel(id);
            accountManagementViewModel.CanDeleteTransaction = canDeleteTransaction;

            return View(accountManagementViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPayment(Guid id, PaymentDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var data = await mediator.SendAsync(new GetImportNotificationAccountOverview(id));
                var accountManagementViewModel = new AccountManagementViewModel(data);
                accountManagementViewModel.RefundViewModel = await GetNewRefundDetailsViewModel(id);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRefund(RefundDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var data = await mediator.SendAsync(new GetImportNotificationAccountOverview(model.NotificationId));
                var accountManagementViewModel = new AccountManagementViewModel(data);
                accountManagementViewModel.PaymentViewModel = new PaymentDetailsViewModel();
                accountManagementViewModel.RefundViewModel = model;
                accountManagementViewModel.ShowRefundDetails = true;

                return View("Index", accountManagementViewModel);
            }

            var refundData = new AddNotificationRefund(model.NotificationId, Convert.ToDecimal(model.RefundAmount),
                model.RefundDate.AsDateTime().Value, model.RefundComments);

            await mediator.SendAsync(refundData);

            return RedirectToAction("index", "AccountManagement", new { id = model.NotificationId });
        }

        private async Task<RefundDetailsViewModel> GetNewRefundDetailsViewModel(Guid id)
        {
            var accountOverview = await mediator.SendAsync(new GetImportNotificationAccountOverview(id));

            var firstPayment =
                accountOverview.Transactions.OrderBy(x => x.Date)
                    .FirstOrDefault(x => x.Transaction == TransactionType.Payment);

            var model = new RefundDetailsViewModel
            {
                NotificationId = id,
                Limit = accountOverview.TotalPaid,
                FirstPaymentReceivedDate = firstPayment == null ? (DateTime?)null : firstPayment.Date
            };

            return model;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, AccountManagementViewModel model, int? commentId)
        {
            if (commentId != null)
            {
                if (string.IsNullOrEmpty(model.Transactions[commentId.GetValueOrDefault()].Comments))
                {
                    model = await this.PrepareViewModel(id);

                    ModelState.AddModelError("CommentError", "Enter a comment");
                    return View(model);
                }

                var result = await mediator.SendAsync(new UpdateImportNotificationAssesmentComments(model.Transactions[commentId.GetValueOrDefault()].TransactionId, model.Transactions[commentId.GetValueOrDefault()].Comments));
            }

            return RedirectToAction("index", "AccountManagement", new { id = id });
        }

        private async Task<AccountManagementViewModel> PrepareViewModel(Guid id)
        {
            var data = await mediator.SendAsync(new GetImportNotificationAccountOverview(id));
            var accountManagementViewModel = new AccountManagementViewModel(data);
            var canDeleteTransaction = await authorizationService.AuthorizeActivity(typeof(DeleteTransaction));

            accountManagementViewModel.PaymentViewModel = new PaymentDetailsViewModel();
            accountManagementViewModel.RefundViewModel = await GetNewRefundDetailsViewModel(id);
            accountManagementViewModel.CanDeleteTransaction = canDeleteTransaction;

            return accountManagementViewModel;
        }
    }
}