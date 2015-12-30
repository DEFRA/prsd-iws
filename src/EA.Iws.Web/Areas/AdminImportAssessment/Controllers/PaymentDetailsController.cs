namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.Transactions;
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
            return View(new PaymentDetailsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, PaymentDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new AddNotificationPayment(id,
                model.Amount.Value,
                model.PaymentMethod,
                model.Date.AsDateTime().Value,
                model.ReceiptNumber,
                model.Comments));

            return RedirectToAction("Index", "AccountManagement");
        } 
    }
}