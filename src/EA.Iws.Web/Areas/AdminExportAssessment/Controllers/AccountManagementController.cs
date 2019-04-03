namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using ViewModels.AccountManagement;
    using ViewModels.PaymentDetails;
    using ViewModels.RefundDetails;

    [AuthorizeActivity(typeof(GetAccountManagementData))]
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
            var data = await mediator.SendAsync(new GetAccountManagementData(id));
            var model = new AccountManagementViewModel(data);
            var canDeleteTransaction = await authorizationService.AuthorizeActivity(typeof(DeleteTransactionController));

            model.PaymentViewModel = new PaymentDetailsViewModel{ NotificationId = id };
            model.RefundViewModel = await GetNewRefundDetailsViewModel(id);
            model.CanDeleteTransaction = canDeleteTransaction;

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> IndexWithError(Guid id, int commentId)
        {
            var data = await mediator.SendAsync(new GetAccountManagementData(id));
            var model = new AccountManagementViewModel(data);
            var canDeleteTransaction = await authorizationService.AuthorizeActivity(typeof(DeleteTransactionController));

            model.PaymentViewModel = new PaymentDetailsViewModel { NotificationId = id };
            model.RefundViewModel = await GetNewRefundDetailsViewModel(id);
            model.CanDeleteTransaction = canDeleteTransaction;

            model.TableData[commentId].Comments = string.Empty;
            model.ErrorCommentId = commentId;
            model.CommentError = "Enter a comment";
            ModelState.AddModelError("CommentError", "Enter a comment");

            return View("index", model);
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
                Date = model.PaymentDate.AsDateTime().Value,
                NotificationId = model.NotificationId,
                Credit = Convert.ToDecimal(model.PaymentAmount),
                PaymentMethod = model.PaymentMethod,
                ReceiptNumber = model.Receipt,
                Comments = model.PaymentComments
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
                Date = model.RefundDate.AsDateTime().Value,
                NotificationId = model.NotificationId,
                Debit = Convert.ToDecimal(model.RefundAmount),
                Comments = model.RefundComments,
                ReceiptNumber = "NA"
            };

            await mediator.SendAsync(new AddNotificationTransaction(refundData));

            return RedirectToAction("index", "AccountManagement", new { id = model.NotificationId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, AccountManagementViewModel model, int? commentId)
        {
            if (commentId != null)
            {
                if (string.IsNullOrEmpty(model.TableData[commentId.GetValueOrDefault()].Comments))
                {
                    return RedirectToAction("IndexWithError", "AccountManagement", new { id = id, commentId = commentId });
                }
                var result = await mediator.SendAsync(new UpdateExportNotificationAssementComments(model.TableData[commentId.GetValueOrDefault()].TransactionId, model.TableData[commentId.GetValueOrDefault()].Comments));
            }

            return RedirectToAction("index", "AccountManagement", new { id = id });
        }
    }
}