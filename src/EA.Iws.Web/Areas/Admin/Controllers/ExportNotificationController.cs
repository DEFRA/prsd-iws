namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Notification;
    using Core.Shared;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin;
    using Requests.Notification;
    using ViewModels.ExportNotification;

    [AuthorizeActivity(typeof(CreateLegacyNotificationApplication))]
    public class ExportNotificationController : Controller
    {
        private readonly IMediator mediator;

        private const string SelectedCompetentAuthority = "CompetentAuthority";
        private const string SelectedNotificationType = "NotifiationType";

        public ExportNotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> NotificationType()
        {
            return View(new NotificationTypeViewModel
            {
                CompetentAuthority = await mediator.SendAsync(new GetUserCompetentAuthority())
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NotificationType(NotificationTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData[SelectedCompetentAuthority] = model.CompetentAuthority;
            TempData[SelectedNotificationType] = model.SelectedNotificationType.GetValueOrDefault();

            return RedirectToAction("NewOrExistingNotification");
        }

        [HttpGet]
        public ActionResult NewOrExistingNotification()
        {
            object competentAuthority;
            object notificationType;

            if (TempData.TryGetValue(SelectedCompetentAuthority, out competentAuthority)
                && TempData.TryGetValue(SelectedNotificationType, out notificationType))
            {
                return View(new NewOrExistingNotificationViewModel
                {
                    CompetentAuthority = (UKCompetentAuthority)competentAuthority,
                    NotificationType = (NotificationType)notificationType
                });
            }

            return RedirectToAction("NotificationType");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewOrExistingNotification(NewOrExistingNotificationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.GenerateNew.Value)
            {
                var response = await mediator.SendAsync(
                    new CreateNotificationApplication
                    {
                        CompetentAuthority = model.CompetentAuthority,
                        NotificationType = model.NotificationType
                    });

                return RedirectToAction("Created", "NewNotification",
                    new
                    {
                        area = string.Empty,
                        id = response
                    });
            }

            TempData[SelectedCompetentAuthority] = model.CompetentAuthority;
            TempData[SelectedNotificationType] = model.NotificationType;

            return RedirectToAction("EnterNumber");
        }

        [HttpGet]
        public ActionResult EnterNumber()
        {
            object competentAuthority;
            object notificationType;

            if (TempData.TryGetValue(SelectedCompetentAuthority, out competentAuthority)
                && TempData.TryGetValue(SelectedNotificationType, out notificationType))
            {
                return View(new EnterNumberViewModel
                {
                    CompetentAuthority = (UKCompetentAuthority)competentAuthority,
                    NotificationType = (NotificationType)notificationType
                });
            }

            return RedirectToAction("NotificationType");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnterNumber(EnterNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await mediator.SendAsync(new CheckNotificationNumberUnique(model.Number.GetValueOrDefault(), model.CompetentAuthority)))
            {
                ModelState.AddModelError("Number", "A record for this notification number already exists");
                return View(model);
            }

            var response = await mediator.SendAsync(
                    new CreateLegacyNotificationApplication
                    {
                        CompetentAuthority = model.CompetentAuthority,
                        NotificationType = model.NotificationType,
                        Number = model.Number.GetValueOrDefault()
                    });

            return RedirectToAction("Created", "NewNotification",
                new
                {
                    area = string.Empty,
                    id = response
                });
        }
    }
}