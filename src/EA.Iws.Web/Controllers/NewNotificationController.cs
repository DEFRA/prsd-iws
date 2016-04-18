namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Notification;
    using Core.Shared;
    using Prsd.Core.Extensions;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using ViewModels.NewNotification;

    [Authorize]
    public class NewNotificationController : Controller
    {
        private readonly IMediator mediator;

        public NewNotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult CompetentAuthority()
        {
            var model = new CompetentAuthorityChoiceViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompetentAuthority(CompetentAuthorityChoiceViewModel model, string cfp)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("NotificationType",
                new
                {
                    ca = model.CompetentAuthorities.SelectedValue,
                    cfp
                });
        }

        [HttpGet]
        public ActionResult NotificationType(string ca, string nt)
        {
            var model = new NotificationTypeViewModel
            {
                CompetentAuthority = ca.GetValueFromDisplayName<UKCompetentAuthority>()
            };

            if (!string.IsNullOrWhiteSpace(nt))
            {
                model.SelectedNotificationType = nt.GetValueFromDisplayName<NotificationType>();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NotificationType(NotificationTypeViewModel model, string cfp)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var response =
                    await
                        mediator.SendAsync(
                            new CreateNotificationApplication
                            {
                                CompetentAuthority = model.CompetentAuthority,
                                NotificationType = model.SelectedNotificationType.Value
                            });

                return RedirectToAction("Created",
                    new
                    {
                        id = response,
                        cfp
                    });
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);

                if (ModelState.IsValid)
                {
                    throw;
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Created(Guid id)
        {
            var response = await mediator.SendAsync(new GetNotificationBasicInfo(id));

            var model = new CreatedViewModel
            {
                NotificationId = response.NotificationId,
                NotificationNumber = response.NotificationNumber,
                CompetentAuthority = response.CompetentAuthority
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Created(CreatedViewModel model, string cfp)
        {
            if (!string.IsNullOrWhiteSpace(cfp) && cfp.Equals("1"))
            {
                return RedirectToAction("Index", "CopyFromNotification",
                    new { id = model.NotificationId });
            }

            return RedirectToAction("Index", "Exporter", new { id = model.NotificationId, area = "NotificationApplication" });
        }
    }
}