namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;
    using ViewModels.Home;

    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId, int? status)
        {
            var movementsSummary =
                await mediator.SendAsync(new GetSummaryAndTable(notificationId, (MovementStatus?)status));

            var model = new MovementSummaryViewModel(notificationId, movementsSummary);
            model.SelectedMovementStatus = (MovementStatus?)status;

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Summary(Guid notificationId)
        {
            var result = mediator
                .SendAsync(new GetBasicMovementSummary(notificationId))
                .GetAwaiter()
                .GetResult();

            return PartialView("_Summary", result);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult CreateSummary(Guid notificationId, int? numberOfNewMovements)
        {
            var result = mediator
                .SendAsync(new GetBasicMovementSummary(notificationId))
                .GetAwaiter()
                .GetResult();

            var shipmentNumbers = ComputeNewShipmentNumbers(numberOfNewMovements, result.TotalShipments);

            var model = new CreateSummaryViewModel
            {
                SummaryData = result,
                NewShipmentNumbers = shipmentNumbers
            };

            return PartialView("_CreateSummary", model);
        }

        private static List<int> ComputeNewShipmentNumbers(int? numberOfNewMovements, int currentTotalMovements)
        {
            var shipmentNumbers = new List<int>();

            if (numberOfNewMovements.HasValue)
            {
                for (int i = currentTotalMovements; i < currentTotalMovements + numberOfNewMovements; i++)
                {
                    shipmentNumbers.Add(i + 1);
                }
            }

            return shipmentNumbers;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public ActionResult IndexPost(Guid notificationId, int? selectedMovementStatus)
        {
            return RedirectToAction("Index", new { notificationId, status = selectedMovementStatus });
        }
    }
}