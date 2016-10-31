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
        public async Task<ActionResult> Index(Guid id)
        {
            var chargeDue = await mediator.SendAsync(new GetChargeDue(id));

            var model = new PaymentDetailsViewModel
            {
                ChargeDue = chargeDue
            };

            return View(model);
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
                model.PaymentAmount.Value,
                model.PaymentMethod,
                model.PaymentDate.AsDateTime().Value,
                model.ReceiptNumber,
                model.PaymentComments));

            return RedirectToAction("Index", "AccountManagement");
        } 
    }
}