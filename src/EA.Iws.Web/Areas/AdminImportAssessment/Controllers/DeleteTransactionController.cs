namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.Transactions;
    using ViewModels.DeleteTransaction;

    [AuthorizeActivity(typeof(DeleteTransaction))]
    public class DeleteTransactionController : Controller
    {
        private readonly IMediator mediator;

        public DeleteTransactionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var transactions = await mediator.SendAsync(new GetImportNotificationTransactions(id));

            var model = new DeleteTransactionViewModel(id, transactions);

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Confirm(Guid id, Guid transactionId)
        {
            var transaction = await mediator.SendAsync(new GetTransactionById(transactionId));

            var model = new ConfirmViewModel(id, transaction);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(ConfirmViewModel model)
        {
            await mediator.SendAsync(new DeleteTransaction(model.NotificationId, model.TransactionId));

            return RedirectToAction("Index", "AccountManagement");
        }
    }
}