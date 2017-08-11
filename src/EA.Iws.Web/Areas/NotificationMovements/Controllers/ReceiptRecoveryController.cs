namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using Core.Movement;
    using Core.Shared;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Movement.Complete;
    using Requests.Movement.Receive;
    using Requests.Notification;
    using Requests.NotificationMovements;
    using Requests.NotificationMovements.Create;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using ViewModels.ReceiptRecovery;

    [AuthorizeActivity(typeof(SaveMovementCompletedReceipt))]
    public class ReceiptRecoveryController : Controller
    {
        private readonly IMediator mediator;
        private readonly IFileReader fileReader;
        private const string DateReceivedKey = "DateReceived";
        private const string DateRecoveredKey = "DateRecovered";
        private const string UnitKey = "Unit";
        private const string QuantityKey = "Quantity";
        private const string ToleranceKey = "Tolerance";
        private const string CertificateKey = "CertificateType";
        private const string NotificationTypeKey = "NotificationType";
        public MovementBasicDetails[] MovementDetails { get; set; }
        public ReceiptRecoveryController(IMediator mediator, IFileReader fileReader)
        {
            this.mediator = mediator;
            this.fileReader = fileReader;
        }

        public async Task<ActionResult> Index(Guid notificationId, Guid[] movementIds)
        {
            ReceiptRecoveryViewModel model = new ReceiptRecoveryViewModel();
            model.SelectedmovementIds = movementIds;
            model.NotificationId = notificationId;
            model.NotificationType = await mediator.SendAsync(new GetNotificationType(notificationId));
            model.Unit = await mediator.SendAsync(new GetShipmentUnits(notificationId));

            if (TempData[CertificateKey] != null)
            {
                model.Certificate = (CertificateType)TempData[CertificateKey];
            }
            else
            {
                return RedirectToAction("CertificateTypes", "Certificate");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid notificationId, Guid[] movementIds, ReceiptRecoveryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData[CertificateKey] = model.Certificate;
            TempData[NotificationTypeKey] = model.NotificationType;

            //Fetch the values for the movementIds
            MovementDetails = await mediator.SendAsync(new GetMovementDetailsByIds(notificationId, movementIds));
            ValidateShipment(model.GetDateReceived());

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var tolerance = await mediator.SendAsync(new DoesQuantityReceivedExceedTolerance(movementIds.FirstOrDefault(), Convert.ToDecimal(model.Quantity), model.Unit));

            TempData[DateReceivedKey] = model.GetDateReceived();
            TempData[UnitKey] = model.Unit;
            TempData[QuantityKey] = model.Quantity;
            TempData[DateRecoveredKey] = model.GetDateRecovered();
            if (tolerance == QuantityReceivedTolerance.AboveTolerance
                || tolerance == QuantityReceivedTolerance.BelowTolerance)
            {
                TempData[ToleranceKey] = tolerance;
                return RedirectToAction("QuantityAbnormal", movementIds.ToRouteValueDictionary("movementIds"));
            }
            
            return RedirectToAction("UploadCertificate", movementIds.ToRouteValueDictionary("movementIds"));
        }

        public async Task<ActionResult> Receipt(Guid notificationId, Guid[] movementIds)
        {
            ReceiptViewModel model = new ReceiptViewModel();
            model.SelectedmovementIds = movementIds;
            model.NotificationId = notificationId;
            model.NotificationType = await mediator.SendAsync(new GetNotificationType(notificationId));
            model.Unit = await mediator.SendAsync(new GetShipmentUnits(notificationId));

            if (TempData[CertificateKey] != null)
            {
                model.Certificate = (CertificateType)TempData[CertificateKey];
            }
            else
            {
                return RedirectToAction("CertificateTypes", "Certificate");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Receipt(Guid notificationId, Guid[] movementIds, ReceiptViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //Fetch the values for the movementIds
            MovementDetails = await mediator.SendAsync(new GetMovementDetailsByIds(notificationId, movementIds));
            //Validate dates
            ValidateShipment(model.GetDateReceived());
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            TempData[CertificateKey] = model.Certificate;
            TempData[NotificationTypeKey] = model.NotificationType;
            var tolerance = await mediator.SendAsync(new DoesQuantityReceivedExceedTolerance(movementIds.FirstOrDefault(), Convert.ToDecimal(model.Quantity), model.Unit));

            TempData[DateReceivedKey] = model.GetDateReceived();
            TempData[UnitKey] = model.Unit;
            TempData[QuantityKey] = model.Quantity;
            if (tolerance == QuantityReceivedTolerance.AboveTolerance
                  || tolerance == QuantityReceivedTolerance.BelowTolerance)
            {
                TempData[ToleranceKey] = tolerance;
                return RedirectToAction("QuantityAbnormal", movementIds.ToRouteValueDictionary("movementIds"));
            }
            return RedirectToAction("UploadCertificate", movementIds.ToRouteValueDictionary("movementIds"));
        }
        public async Task<ActionResult> Recovery(Guid notificationId, Guid[] movementIds)
        {
            RecoveryViewModel model = new RecoveryViewModel();
            model.SelectedmovementIds = movementIds;
            model.NotificationId = notificationId;
            model.NotificationType = await mediator.SendAsync(new GetNotificationType(notificationId));
            if (TempData[CertificateKey] != null)
            {
                model.Certificate = (CertificateType)TempData[CertificateKey];
            }
            else
            {
                return RedirectToAction("CertificateTypes", "Certificate");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Recovery(Guid notificationId, Guid[] movementIds, RecoveryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            TempData[CertificateKey] = model.Certificate;
            TempData[NotificationTypeKey] = model.NotificationType;
            TempData[DateRecoveredKey] = model.GetDateRecovered();
            //Fetch the values for the movementIds
            MovementDetails = await mediator.SendAsync(new GetMovementDetailsByIds(notificationId, movementIds));
            //Validate dates
            ValidateRecoveryShipment(model.GetDateRecovered());
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            return RedirectToAction("UploadCertificate", movementIds.ToRouteValueDictionary("movementIds"));
        }

        [HttpGet]
        public ActionResult QuantityAbnormal(Guid notificationId, Guid[] movementIds)
        {
            object date;
            object quantity;
            object unit;
            object tolerance;
            if (TempData.TryGetValue(DateReceivedKey, out date)
                && TempData.TryGetValue(QuantityKey, out quantity)
                && TempData.TryGetValue(UnitKey, out unit)
                && TempData.TryGetValue(ToleranceKey, out tolerance))
            {
                return View(new QuantityAbnormalViewModel
                {
                    Quantity = Convert.ToDecimal(quantity),
                    Tolerance = (QuantityReceivedTolerance)tolerance,
                    Unit = (ShipmentQuantityUnits)unit,
                    DateReceived = Convert.ToDateTime(date),
                    DateRecovered = TempData[DateRecoveredKey] != null ? Convert.ToDateTime(TempData[DateRecoveredKey]) : (DateTime?)null,
                    Certificate = (CertificateType)TempData[CertificateKey],
                    NotificationType = (NotificationType)TempData[NotificationTypeKey],
                    NotificationId = notificationId
            });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuantityAbnormal(Guid notificationId, Guid[] movementIds, QuantityAbnormalViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData[DateReceivedKey] = model.DateReceived;
            TempData[UnitKey] = model.Unit;
            TempData[QuantityKey] = model.Quantity;
            TempData[DateRecoveredKey] = model.DateRecovered;
            TempData[CertificateKey] = model.Certificate;
            TempData[NotificationTypeKey] = model.NotificationType;

            return RedirectToAction("UploadCertificate", movementIds.ToRouteValueDictionary("movementIds"));
        }

        [HttpGet]
        public ActionResult UploadCertificate(Guid notificationId, Guid[] movementIds)
        {
            //Check all the values are available
            object dateReceivedResult;
            object unitResult;
            object quantityResult;

            var model = new UploadCertificateViewModel();

            model.Certificate = (CertificateType)TempData[CertificateKey];
            model.NotificationType = (NotificationType)TempData[NotificationTypeKey];
            model.NotificationId = notificationId;
            model.MovementIds = movementIds;

            if (model.Certificate == CertificateType.Receipt || model.Certificate == CertificateType.ReceiptRecovery)
            {
                if (TempData.TryGetValue(DateReceivedKey, out dateReceivedResult)
                    && TempData.TryGetValue(UnitKey, out unitResult)
                    && TempData.TryGetValue(QuantityKey, out quantityResult))
                {
                    model.DateReceived = DateTime.Parse(dateReceivedResult.ToString());
                    model.Unit = (ShipmentQuantityUnits)unitResult;
                    model.Quantity = decimal.Parse(quantityResult.ToString());
                }
            }
            if (model.Certificate == CertificateType.Recovery || model.Certificate == CertificateType.ReceiptRecovery)
            {
                model.DateRecovered = DateTime.Parse(TempData[DateRecoveredKey].ToString());
            }   
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadCertificate(Guid notificationId, Guid[] movementIds, UploadCertificateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.MovementIds = movementIds;
            if (model.Certificate == CertificateType.Receipt)
            {
                await SaveReceiptData(notificationId, model);
            }
            else if (model.Certificate == CertificateType.Recovery)
            {
                await SaveCompleteData(notificationId, model);
            }
            else if (model.Certificate == CertificateType.ReceiptRecovery)
            {
                await SaveReceiptData(notificationId, model);
                await SaveCompleteData(notificationId, model);
            }
            TempData[CertificateKey] = model.Certificate;
            TempData[NotificationTypeKey] = model.NotificationType;

            return RedirectToAction("Success");
        }

        private async Task SaveReceiptData(Guid notificationId, UploadCertificateViewModel model)
        {
            var fileExtension = Path.GetExtension(model.File.FileName);
            var uploadedFile = await fileReader.GetFileBytes(model.File);
            for (int i = 0; i < model.MovementIds.Count(); i++)
            {
                var id = model.MovementIds.ElementAt(i);
                var fileId = await mediator.SendAsync(new SaveCertificateOfReceiptFile(id, uploadedFile, fileExtension));

                await mediator.SendAsync(new SetMovementAccepted(id, fileId, model.DateReceived, model.Quantity.GetValueOrDefault(), model.Unit.GetValueOrDefault()));
            }
        }

        private async Task SaveCompleteData(Guid notificationId, UploadCertificateViewModel model)
        {
            var fileExtension = Path.GetExtension(model.File.FileName);
            var uploadedFile = await fileReader.GetFileBytes(model.File);

            for (int i = 0; i < model.MovementIds.Count(); i++)
            {
                var id = model.MovementIds.ElementAt(i);

                await mediator.SendAsync(new SaveMovementCompletedReceipt(id, model.DateRecovered, uploadedFile, fileExtension));
            }
        }

        [HttpGet]
        public ActionResult Success(Guid notificationId)
        {
            object notificationTypeResult;
            object certificateResult;

            if (TempData.TryGetValue(NotificationTypeKey, out notificationTypeResult) &&
            TempData.TryGetValue(CertificateKey, out certificateResult))
            {
                var model = new SuccessViewModel
                {
                    NotificationId = notificationId,
                    NotificationType = (NotificationType)notificationTypeResult,
                    Certificate = (CertificateType)certificateResult
                };

                return View(model);
            }
            return View();
        }

        public void ValidateShipment(DateTime dateReceived)
        {
            for (int i = 0; i < MovementDetails.Count(); i++)
            {
                if (dateReceived < MovementDetails[i].ActualDate)
                {
                    ModelState.AddModelError("Day", "This date cannot be before the actual date of shipment. Please enter a different date for shipment(s) - " + MovementDetails[i].Number);
                }
            }
        }

        public void ValidateRecoveryShipment(DateTime dateComplete)
        {
            for (int i = 0; i < MovementDetails.Count(); i++)
            {
                if (dateComplete < MovementDetails[i].ReceiptDate)
                {
                    ModelState.AddModelError("Day", "This date cannot be before the date of receipt. Please enter a different date for shipment(s) - " + MovementDetails[i].Number);
                }
            }
        }
    }
}