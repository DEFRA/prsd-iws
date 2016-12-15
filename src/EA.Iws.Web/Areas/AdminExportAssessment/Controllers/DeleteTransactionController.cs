namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using ViewModels.DeleteTransaction;

    [AuthorizeActivity(typeof(Requests.ImportNotificationAssessment.Transactions.DeleteTransaction))]
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
            var transactions = await mediator.SendAsync(new GetExportNotificationTransactions(id));

            var model = new DeleteTransactionViewModel(id, transactions);

            return View(model);
        }
    }
}