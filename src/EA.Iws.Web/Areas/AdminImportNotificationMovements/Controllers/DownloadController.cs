namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.ImportNotificationMovements;

    [Authorize(Roles = "internal")]
    public class DownloadController : Controller
    {
        private readonly IMediator mediator;

        public DownloadController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Excel(Guid id)
        {
            var notification = await mediator.SendAsync(new GetNotificationDetails(id));
            var movementData = await mediator.SendAsync(new GetMovementsByNotificationId(id));
            var data = movementData.Select(movement => new DownloadMovementData
            {
                Number = movement.Number,
                Status = movement.Status,
                SubmittedDate = movement.SubmittedDate,
                ShipmentDate = movement.ShipmentDate,
                ReceivedDate = movement.ReceivedDate,
                Quantity = movement.Quantity,
                QuantityUnits = movement.QuantityUnits,
                CompletedDate = movement.CompletedDate
            }).ToList();

            var filename = string.Format("movement-details-for-notification-number-{0}.xlsx", notification.NotificationNumber);

            return new XlsxActionResult<DownloadMovementData>(data, filename);
        }
    }
}