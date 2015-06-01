namespace EA.Iws.Web.Controllers
{
    using System;
    using System.IdentityModel.Protocols.WSTrust;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core;
    using Prsd.Core.Extensions;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using Requests.Shared;
    using ViewModels.NotificationApplication;
    using ViewModels.Shared;
    using Constants = Prsd.Core.Web.Constants;

    [Authorize]
    public class NotificationApplicationController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public NotificationApplicationController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public ActionResult CompetentAuthority()
        {
            var model = new CompetentAuthorityChoiceViewModel
            {
                CompetentAuthorities =
                    RadioButtonStringCollectionViewModel.CreateFromEnum<CompetentAuthority>()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompetentAuthority(CompetentAuthorityChoiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("NotificationTypeQuestion",
                new { ca = model.CompetentAuthorities.SelectedValue });
        }

        [HttpGet]
        public ActionResult NotificationTypeQuestion(string ca, string nt)
        {
            var model = new InitialQuestionsViewModel
            {
                SelectedNotificationType = NotificationType.Recovery,
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
        public async Task<ActionResult> NotificationTypeQuestion(InitialQuestionsViewModel model)
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
                            id = response
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
        public ActionResult Created(CreatedViewModel model)
        {
            return RedirectToAction(actionName: "DownloadInstructions", controllerName: "NotificationApplication",
                routeValues: new { id = model.NotificationId });
        }

        [HttpGet]
        public async Task<ActionResult> GenerateNotificationDocument(Guid id)
        {
            using (var client = apiClient())
            {
                try
                {
                    var response =
                        await client.SendAsync(User.GetAccessToken(), new GenerateNotificationDocument(id));

                    var downloadName = "IwsNotification" + SystemTime.UtcNow + ".docx";

                    return File(response, Constants.MicrosoftWordContentType, downloadName);
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);
                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                    return HttpNotFound();
                }
            }
        }

        public ActionResult _GetUserNotifications()
        {
            using (var client = apiClient())
            {
                // Child actions (partial views) cannot be async and we must therefore get the result of the task.
                // The called code must use ConfigureAwait(false) on async tasks to prevent deadlock.
                var response =
                    client.SendAsync(User.GetAccessToken(), new GetNotificationsByUser()).Result;

                return PartialView(response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> DownloadInstructions(Guid id)
        {
            using (var client = apiClient())
            {
                var response = await client.SendAsync(User.GetAccessToken(), new GetNotificationInfo(id));

                var model = new NotificationInfo
                {
                    CompetentAuthority = response.CompetentAuthority,
                    NotificationId = id,
                    NotificationNumber = response.NotificationNumber
                };

                switch (response.CompetentAuthority)
                {
                    case Requests.Notification.CompetentAuthority.England:
                        model.CompetentAuthorityName = "the Environment Agency (England)";
                        break;
                    case Requests.Notification.CompetentAuthority.Scotland:
                        model.CompetentAuthorityName = "the Scottish Environment Protection Agency";
                        break;
                    case Requests.Notification.CompetentAuthority.NorthernIreland:
                        model.CompetentAuthorityName = "the Northern Ireland Environment Agency";
                        break;
                    case Requests.Notification.CompetentAuthority.Wales:
                        model.CompetentAuthorityName = "Natural Resources Wales";
                        break;
                    default:
                        throw new InvalidOperationException(string.Format("Unknown competent authority: {0}",
                            response.CompetentAuthority));
                }

                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> NotificationOverview(Guid id)
        {
            using (var client = apiClient())
            {
                var response = await client.SendAsync(User.GetAccessToken(), new GetNotificationInfo(id));

                var model = new NotificationOverviewViewModel
                {
                    NotificationId = response.NotificationId,
                    NotificationNumber = response.NotificationNumber,
                    NotificationType = response.NotificationType                    
                };
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult SpecialHandling(Guid id)
        {
            return View(new SpecialHandlingViewModel{NotificationId = id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SpecialHandling(SpecialHandlingViewModel model)
        {
            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), new SetSpecialHandling(model.NotificationId, model.IsSpecialHandling, model.SpecialHandlingDetails));
            }
            return RedirectToAction("Info", "Shipment", new { id = model.NotificationId });
        }
    }
}