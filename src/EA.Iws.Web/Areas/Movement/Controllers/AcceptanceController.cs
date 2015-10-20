namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.MovementReceipt;
    using Prsd.Core.Mediator;
    using ViewModels.Acceptance;

    [Authorize]
    public class AcceptanceController : Controller
    {
        private const string DateReceivedKey = "DateReceived";
        private const string RejectionReasonKey = "RejectionReason";
        private const string DecisionKey = "Decision";

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            object dateReceivedResult;

            if (TempData.TryGetValue(DateReceivedKey, out dateReceivedResult))
            {
                var model = new AcceptanceViewModel { DateReceived = DateTime.Parse(dateReceivedResult.ToString()) };

                return View(model);
            }
            
            return RedirectToAction("Index", "DateReceived", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid id, AcceptanceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData[DateReceivedKey] = model.DateReceived;
            TempData[DecisionKey] = model.Decision;

            if (model.Decision == Decision.Rejected)
            {
                TempData[RejectionReasonKey] = model.RejectReason;

                return RedirectToAction("Index", "ReceiptComplete", new { id });
            }

            return RedirectToAction("Index", "QuantityReceived", new { id });
        }
    }
}