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
    using Requests.NotificationMovements.Create;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
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

        public ReceiptRecoveryController(IMediator mediator, IFileReader fileReader)
        {
            this.mediator = mediator;
            this.fileReader = fileReader;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId, Guid movementId)
        {
            ReceiptRecoveryViewModel model = new ReceiptRecoveryViewModel();
            model.SelectedmovementId = movementId;
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
        public async Task<ActionResult> Index(Guid notificationId, Guid movementId, ReceiptRecoveryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData[CertificateKey] = model.Certificate;
            TempData[NotificationTypeKey] = model.NotificationType;

            MovementBasicDetails movementDetails = await mediator.SendAsync(new GetMovementDetailsById(notificationId, movementId));
            ValidateShipment(model.GetDateReceived(), movementDetails);

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            TempData[DateReceivedKey] = model.GetDateReceived();
            TempData[UnitKey] = model.Unit;
            TempData[QuantityKey] = model.Quantity;
            TempData[DateRecoveredKey] = model.GetDateRecovered();

            var shipmentExists = await mediator.SendAsync(new DoesMovementDetailsExist(movementId));

            if (shipmentExists)
            {
                var tolerance = await mediator.SendAsync(new DoesQuantityReceivedExceedTolerance(movementId, Convert.ToDecimal(model.Quantity), model.Unit));

                if (tolerance == QuantityReceivedTolerance.AboveTolerance
                || tolerance == QuantityReceivedTolerance.BelowTolerance)
                {
                    TempData[ToleranceKey] = tolerance;
                    return RedirectToAction("QuantityAbnormal", new { movementId = movementId });
                }
            }
            
            return RedirectToAction("UploadCertificate", new { movementId = movementId });
        }

        [HttpGet]
        public async Task<ActionResult> Receipt(Guid notificationId, Guid movementId)
        {
            ReceiptViewModel model = new ReceiptViewModel();
            model.SelectedmovementId = movementId;
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
        public async Task<ActionResult> Receipt(Guid notificationId, Guid movementId, ReceiptViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            MovementBasicDetails movementDetails = await mediator.SendAsync(new GetMovementDetailsById(notificationId, movementId));

            ValidateShipment(model.GetDateReceived(), movementDetails);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            TempData[CertificateKey] = model.Certificate;
            TempData[NotificationTypeKey] = model.NotificationType;
            TempData[DateReceivedKey] = model.GetDateReceived();
            TempData[UnitKey] = model.Unit;
            TempData[QuantityKey] = model.Quantity;

            var shipmentExists = await mediator.SendAsync(new DoesMovementDetailsExist(movementId));

            if (shipmentExists)
            {
                var tolerance = await mediator.SendAsync(new DoesQuantityReceivedExceedTolerance(movementId, Convert.ToDecimal(model.Quantity), model.Unit));
                
                if (tolerance == QuantityReceivedTolerance.AboveTolerance
                      || tolerance == QuantityReceivedTolerance.BelowTolerance)
                {
                    TempData[ToleranceKey] = tolerance;
                    return RedirectToAction("QuantityAbnormal", new { movementId = movementId });
                }
            }

            return RedirectToAction("UploadCertificate", new { movementId = movementId});
        }
        [HttpGet]
        public async Task<ActionResult> Recovery(Guid notificationId, Guid movementId)
        {
            RecoveryViewModel model = new RecoveryViewModel();
            model.SelectedmovementId = movementId;
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
        public async Task<ActionResult> Recovery(Guid notificationId, Guid movementId, RecoveryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            TempData[CertificateKey] = model.Certificate;
            TempData[NotificationTypeKey] = model.NotificationType;
            TempData[DateRecoveredKey] = model.GetDateRecovered();

            MovementBasicDetails movementDetails = await mediator.SendAsync(new GetMovementDetailsById(notificationId, movementId));
            
            ValidateRecoveryShipment(model.GetDateRecovered(), movementDetails);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            return RedirectToAction("UploadCertificate", new { movementId = movementId });
        }

        [HttpGet]
        public ActionResult QuantityAbnormal(Guid notificationId, Guid movementId)
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

            return RedirectToAction("CertificateTypes", "Certificate");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuantityAbnormal(Guid notificationId, Guid movementId, QuantityAbnormalViewModel model)
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

            return RedirectToAction("UploadCertificate", new { movementId = movementId });
        }

        [HttpGet]
        public ActionResult UploadCertificate(Guid notificationId, Guid movementId)
        {
            //Check all the values are available
            object dateReceivedResult;
            object unitResult;
            object quantityResult;

            var model = new UploadCertificateViewModel();

            model.Certificate = (CertificateType)TempData[CertificateKey];
            model.NotificationType = (NotificationType)TempData[NotificationTypeKey];
            model.NotificationId = notificationId;
            model.MovementId = movementId;

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
        public async Task<ActionResult> UploadCertificate(Guid notificationId, Guid movementId, UploadCertificateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.MovementId = movementId;
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
                await SaveReceiptRecoveryData(notificationId, model);
            }
            TempData[CertificateKey] = model.Certificate;
            TempData[NotificationTypeKey] = model.NotificationType;

            return RedirectToAction("Success");
        }

        private async Task SaveReceiptData(Guid notificationId, UploadCertificateViewModel model)
        {
            var fileExtension = Path.GetExtension(model.File.FileName);
            var uploadedFile = await fileReader.GetFileBytes(model.File);

            var id = model.MovementId;
           var fileId = await mediator.SendAsync(new SaveCertificateOfReceiptFile(id, uploadedFile, fileExtension));

            await mediator.SendAsync(new SetMovementAccepted(id, fileId, model.DateReceived, model.Quantity.GetValueOrDefault(), model.Unit.GetValueOrDefault()));           
        }

        private async Task SaveCompleteData(Guid notificationId, UploadCertificateViewModel model)
        {
            var fileExtension = Path.GetExtension(model.File.FileName);
            var uploadedFile = await fileReader.GetFileBytes(model.File);

            var id = model.MovementId;

            await mediator.SendAsync(new SaveMovementCompletedReceipt(id, model.DateRecovered, uploadedFile, fileExtension));           
        }

        private async Task SaveReceiptRecoveryData(Guid notificationId, UploadCertificateViewModel model)
        {
            var fileExtension = Path.GetExtension(model.File.FileName);
            var uploadedFile = await fileReader.GetFileBytes(model.File);

            var id = model.MovementId;
            var fileId = await mediator.SendAsync(new SaveCertificateOfReceiptFile(id, uploadedFile, fileExtension));

            await mediator.SendAsync(new SetMovementAccepted(id, fileId, model.DateReceived, model.Quantity.GetValueOrDefault(), model.Unit.GetValueOrDefault()));
            await mediator.SendAsync(new SaveMovementCompletedReceipt(id, model.DateRecovered, uploadedFile, fileExtension));
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

        private void ValidateShipment(DateTime dateReceived, MovementBasicDetails movementDetails)
        {           
            if (dateReceived < movementDetails.ActualDate)
            {
                ModelState.AddModelError("Day", "This date cannot be before the actual date of shipment. Please enter a different date for shipment - " + movementDetails.Number);
            }            
        }

        private void ValidateRecoveryShipment(DateTime dateComplete, MovementBasicDetails movementDetails)
        {        
            if (dateComplete < movementDetails.ReceiptDate)
            {
                ModelState.AddModelError("Day", "This date cannot be before the date of receipt. Please enter a different date for shipment - " + movementDetails.Number);
            }
        }
    }
}