namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using ViewModels.PaymentDetails;

    [Authorize(Roles = "internal")]
    public class PaymentDetailsController : Controller
    {
        private readonly IMediator mediator;

        public PaymentDetailsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
            
        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var model = new PaymentDetailsViewModel { NotificationId = id };

            ViewBag.ActiveSection = "Finance";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(PaymentDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActiveSection = "Finance";
                return View(model);
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