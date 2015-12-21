namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.ImportNotificationAssessment;
    using ViewModels.ImportNotification;

    [Authorize]
    public class ImportNotificationController : Controller
    {
        private readonly IMediator mediator;
        private const string ImportNotificationNumber = "ImportNotificationNumber";
        private const string ImportNotificationReceivedDate = "ReceivedDate";

        public ImportNotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Number()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Number(NotificationNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await mediator.SendAsync(new CheckImportNumberUnique(model.NotificationNumber)))
            {
                ModelState.AddModelError("NotificationNumber", "A record for this notification number already exists");
                return View(model);
            }

            TempData[ImportNotificationNumber] = model.NotificationNumber;
            return RedirectToAction("ReceivedDate");
        }

        [HttpGet]
        public ActionResult ReceivedDate()
        {
            object number;
            if (TempData.TryGetValue(ImportNotificationNumber, out number))
            {
                return View(new NotificationReceivedDateViewModel
                {
                    NotificationNumber = number.ToString()
                });
            }

            return RedirectToAction("Number");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReceivedDate(NotificationReceivedDateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData[ImportNotificationNumber] = model.NotificationNumber;
            TempData[ImportNotificationReceivedDate] = model.NotificationReceived.AsDateTime().Value;

            return RedirectToAction("NotificationType");
        }

        [HttpGet]
        public ActionResult NotificationType()
        {
            object number;
            object receivedDate;
            if (!TempData.TryGetValue(ImportNotificationNumber, out number))
            {
                return RedirectToAction("Number");
            }

            if (!TempData.TryGetValue(ImportNotificationReceivedDate, out receivedDate))
            {
                return RedirectToAction("ReceivedDate");
            }

            return View(new NotificationTypeViewModel
            {
                NotificationNumber = number.ToString(),
                ReceivedDate = (DateTime)receivedDate
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NotificationType(NotificationTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var notificationType = (NotificationType)model.NotificationTypeRadioButtons.SelectedValue;

            var id = await mediator.SendAsync(new CreateImportNotification(model.NotificationNumber, notificationType));

            await mediator.SendAsync(new SetReceivedDate(id, model.ReceivedDate));

            if (notificationType == Core.Shared.NotificationType.Disposal)
            {
                return RedirectToAction("Index", "Exporter", new { area = "ImportNotification", id });
            }

            return RedirectToAction("Index", "Preconsented", new { area = "ImportNotification", id });
        }
    }
}