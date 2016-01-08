namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using ViewModels.RefundDetails;

    [Authorize(Roles = "internal")]
    public class RefundDetailsController : Controller
    {
        private readonly IMediator mediator;

        public RefundDetailsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var limit = await mediator.SendAsync(new GetRefundLimit(id));

            var model = new RefundDetailsViewModel
            {
                NotificationId = id,
                Limit = limit
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(RefundDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
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