namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using ViewModels.ImportNotification;

    [Authorize(Roles = "internal")]
    public class ImportNotificationController : Controller
    {
        private readonly IMediator mediator;
        private const string ImportNotificationNumber = "ImportNotificationNumber";
        private const string ImportNotificationReceivedDate = "ReceivedDate";
        private const string ImportNotificationInterim = "IsInterim";

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
        public ActionResult Interim()
        {
            object number;
            object receivedDate;
            if (TempData.TryGetValue(ImportNotificationNumber, out number)
                && TempData.TryGetValue(ImportNotificationReceivedDate, out receivedDate))
            {
                return View(new NotificationInterimViewModel
                {
                    NotificationNumber = number.ToString(),
                    NotificationReceivedDate = (DateTime)receivedDate
                });
            }

            return RedirectToAction("Number");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Interim(NotificationInterimViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData[ImportNotificationNumber] = model.NotificationNumber;
            TempData[ImportNotificationReceivedDate] = model.NotificationReceivedDate;
            TempData[ImportNotificationInterim] = model.IsInterim;

            return RedirectToAction("NotificationType");
        }

        [HttpGet]
        public ActionResult NotificationType()
        {
            object number;
            object receivedDate;
            object isInterim;
            if (!TempData.TryGetValue(ImportNotificationNumber, out number))
            {
                return RedirectToAction("Number");
            }

            if (!TempData.TryGetValue(ImportNotificationReceivedDate, out receivedDate))
            {
                return RedirectToAction("ReceivedDate");
            }

            if (!TempData.TryGetValue(ImportNotificationInterim, out isInterim))
            {
                return RedirectToAction("Interim");
            }

            return View(new NotificationTypeViewModel
            {
                NotificationNumber = number.ToString(),
                ReceivedDate = (DateTime)receivedDate,
                IsInterim = (bool)isInterim
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

            var id = await mediator.SendAsync(new CreateImportNotification(model.NotificationNumber, notificationType, model.ReceivedDate, model.IsInterim));

            if (notificationType == Core.Shared.NotificationType.Disposal)
            {
                return RedirectToAction("Index", "Exporter", new { area = "ImportNotification", id });
            }

            return RedirectToAction("Index", "Preconsented", new { area = "ImportNotification", id });
        }
    }
}