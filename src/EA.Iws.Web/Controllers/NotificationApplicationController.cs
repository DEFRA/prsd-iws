namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Api.Client.Entities;
    using Infrastructure;
    using ViewModels.NotificationApplication;
    using ViewModels.Shared;

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
            var model = new CompetentAuthorityChoice
            {
                CompetentAuthorities =
                    RadioButtonStringCollection.CreateFromEnum<CompetentAuthority>()
            };

            return View("CompetentAuthority", model);
        }

        [HttpPost]
        public ActionResult CompetentAuthority(CompetentAuthorityChoice model)
        {
            if (!ModelState.IsValid)
            {
                return View("CompetentAuthority", model);
            }

            return RedirectToAction("WasteActionQuestion",
                new { ca = model.CompetentAuthorities.SelectedValue });
        }

        [HttpGet]
        public ActionResult WasteActionQuestion(string ca, string nt)
        {
            InitialQuestions model = new InitialQuestions
            {
                SelectedWasteAction = WasteAction.Recovery,
                CompetentAuthority = ca.GetValueFromDisplayName<CompetentAuthority>()
            };
            
            if (!string.IsNullOrWhiteSpace(nt))
            {
                model.SelectedWasteAction = nt.GetValueFromDisplayName<WasteAction>();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> WasteActionQuestion(InitialQuestions model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                var response = await client.Notification.CreateNotificationApplicationAsync(User.GetAccessToken(),
                    model.ToNotificationData());

                if (!response.HasErrors)
                {
                    return RedirectToAction("Created",
                        new
                        {
                            id = response.Result
                        });
                }
                else
                {
                    this.AddValidationErrorsToModelState(response);
                    return View(model);
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> Created(Guid id)
        {
            using (var client = apiClient())
            {
                var notification = await client.Notification.GetNotificationInformationAsync(User.GetAccessToken(), id);
                var model = new Created()
                {
                    NotificationId = notification.Id,
                    NotificationNumber = notification.NotificationNumber
                };
                return View(model);
            }
        }
    }
}