namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.Notification;
    using Core.Shared;
    using Infrastructure;
    using Prsd.Core.Extensions;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using ViewModels.NewNotification;

    public class NewNotificationController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public NewNotificationController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
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
                CompetentAuthority = ca.GetValueFromDisplayName<CompetentAuthority>()
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

            using (var client = apiClient())
            {
                try
                {
                    var response =
                        await
                            client.SendAsync(User.GetAccessToken(),
                                new CreateNotificationApplication
                                {
                                    CompetentAuthority = model.CompetentAuthority,
                                    NotificationType = model.SelectedNotificationType
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
        }

        [HttpGet]
        public async Task<ActionResult> Created(Guid id)
        {
            using (var client = apiClient())
            {
                var response = await client.SendAsync(User.GetAccessToken(), new GetNotificationInfo(id));

                var model = new CreatedViewModel
                {
                    NotificationId = response.NotificationId,
                    NotificationNumber = response.NotificationNumber
                };
                return View(model);
            }
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