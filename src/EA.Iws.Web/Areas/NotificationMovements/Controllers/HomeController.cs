namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mediator;
    using Requests.Movement.Summary;
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
        public async Task<ActionResult> Index(Guid notificationId)
        {
            var movementsSummary = await mediator.SendAsync(new GetMovementsSummaryByNotificationId(notificationId));

            var tableDataList = new List<MovementSummaryTableViewModel>();

            foreach (var mstd in movementsSummary.ShipmentTableData)
            {
                var tableData = new MovementSummaryTableViewModel(mstd);
                
                tableDataList.Add(tableData);
            }

            var model = new MovementSummaryViewModel(notificationId, movementsSummary, tableDataList);
            
            return View(model);
        }
    }
}