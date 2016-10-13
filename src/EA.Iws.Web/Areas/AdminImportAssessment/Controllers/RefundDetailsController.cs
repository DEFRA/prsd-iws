namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.Transactions;
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
            var accountOverview = await mediator.SendAsync(new GetImportNotificationAccountOverview(id));

            var model = new RefundDetailsViewModel
            {
                NotificationId = id,
                Limit = accountOverview.TotalPaid,
                PaymentReceivedDate = accountOverview.PaymentReceived
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

            var refundData = new AddNotificationRefund(model.NotificationId, Convert.ToDecimal(model.Amount),
                model.Date.AsDateTime().Value, model.Comments);

            await mediator.SendAsync(refundData);

            return RedirectToAction("index", "AccountManagement", new { id = model.NotificationId });
        }
    }
}