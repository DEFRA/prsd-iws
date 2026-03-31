namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Core.NotificationAssessment;
    using EA.Iws.Requests.NotificationAssessment;
    using EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.Home;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Notification;
    using Requests.NotificationMovements;
    using Requests.NotificationMovements.Capture;
    using Web.ViewModels.Shared;

    [AuthorizeActivity(typeof(GetSummaryAndTable))]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;
        private readonly AuthorizationService authorizationService;
        // TempData stored in the Cancel controller
        private const string SubmittedMovementListKey = "SubmittedMovementListKey";
        private const string AddedCancellableMovementsListKey = "AddedCancellableMovementsListKey";

        public string PreNotificationWarnings { get; set; }
        public string EarlyShipmentWarnings { get; set; }
        public string ConsentedDateWarnings { get; set; }
        public List<MovementSummaryTableViewModel> TableData { get; set; }
        public List<NotificationAssessmentDecision> Decisions { get; set; }

        public HomeController(IMediator mediator, AuthorizationService authorizationService)
        {
            this.mediator = mediator;
            this.authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, int? status, int page = 1)
        {
            TempData[SubmittedMovementListKey] = null;
            TempData[AddedCancellableMovementsListKey] = null;

            var movementsSummary = await mediator.SendAsync(new GetSummaryAndTable(id, (MovementStatus?)status, page));
            var canDeleteMovement = await authorizationService.AuthorizeActivity(typeof(DeleteMovement));
            var keyDates = await mediator.SendAsync(new GetKeyDatesSummaryInformation(id));

            TableData = new List<MovementSummaryTableViewModel>(
                movementsSummary.ShipmentTableData.OrderByDescending(m => m.Number)
                    .Select(p => new MovementSummaryTableViewModel(p)));
            Decisions = new List<NotificationAssessmentDecision>(
                keyDates.DecisionHistory.Where(d => d.Status == EA.Iws.Core.NotificationAssessment.NotificationStatus.Consented));

            PreNotificationWarnings = GetPreNotificationWarnings(TableData);
            EarlyShipmentWarnings = GetEarlyShipmentWarnings(TableData);
            ConsentedDateWarnings = GetConsentedDateExceededWarnings(Decisions, TableData);

            var model = new MovementSummaryViewModel(id, movementsSummary, PreNotificationWarnings, EarlyShipmentWarnings, ConsentedDateWarnings);

            model.SelectedMovementStatus = (MovementStatus?)status;
            model.CanDeleteMovement = canDeleteMovement && movementsSummary.SummaryData.NotificationStatus != NotificationStatus.FileClosed;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public ActionResult IndexPost(Guid id, int? selectedMovementStatus)
        {
            return RedirectToAction("Index", new { id, status = selectedMovementStatus, page = 1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Search(Guid id, int? shipmentNumber)
        {
            if (!shipmentNumber.HasValue || shipmentNumber.Value <= 0)
            {
                return RedirectToAction("Index");
            }

            var movementId = await mediator.SendAsync(new GetMovementIdIfExists(id, shipmentNumber.Value));
            if (movementId.HasValue)
            {
                return RedirectToAction("Edit", "CaptureMovement", new { movementId });
            }
            else
            {
                return RedirectToAction("Create", "CaptureMovement", new { shipmentNumber });
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult NotificationSwitcher(Guid id)
        {
            var response = Task.Run(() => mediator.SendAsync(new GetNotificationNumber(id))).Result;

            return PartialView("_NotificationSwitcher", new NotificationSwitcherViewModel(response));
        }

        private string GetPreNotificationWarnings(List<MovementSummaryTableViewModel> tableData)
        {
            var warnings = new StringBuilder();

            foreach (var row in tableData)
            {
                DateTime? preNotDate = row.PreNotification;
                DateTime? shipDate = row.ShipmentDate;

                if (preNotDate.HasValue && shipDate.HasValue)
                {
                    var difference = (shipDate.Value.Date - preNotDate.Value.Date).Days;

                    if (difference < 3)
                    {
                        warnings.Append(", " + row.Number.ToString());
                    }
                }
            }

            if (warnings.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return " for shipments: " + warnings.ToString().Remove(0, 2);
            }
        }

        private string GetEarlyShipmentWarnings(List<MovementSummaryTableViewModel> tableData)
        {
            var warnings = new StringBuilder();

            foreach (var row in tableData)
            {
                DateTime? shipDate = row.ShipmentDate;
                DateTime? receivedDate = row.Received;

                if (shipDate.HasValue && receivedDate.HasValue)
                {
                    if (DateTime.Compare((DateTime)shipDate, (DateTime)receivedDate) > 0)
                    {
                        warnings.Append(", " + row.Number.ToString());
                    }
                }
            }

            if (warnings.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return " for shipments: " + warnings.ToString().Remove(0, 2);
            }
        }

        private string GetConsentedDateExceededWarnings(List<NotificationAssessmentDecision> decisions, List<MovementSummaryTableViewModel> tableData)
        {
            var warnings = new StringBuilder();

            if (decisions != null && decisions.Any())
            {
                var mostRecentConsentedDecision = decisions.OrderByDescending(d => d.Date).FirstOrDefault(d => d.Status == NotificationStatus.Consented);
                var mostRecentConsentedDate = mostRecentConsentedDecision.Date;

                foreach (var row in tableData)
                {
                    DateTime? shipDate = row.ShipmentDate;

                    if (shipDate.HasValue)
                    {
                        if (DateTime.Compare((DateTime)shipDate, (DateTime)mostRecentConsentedDate) > 0)
                        {
                            warnings.Append(", " + row.Number.ToString());
                        }
                    }
                }
            }

            if (warnings.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return " for shipments: " + warnings.ToString().Remove(0, 2);
            }
        }
    }
}