namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using ViewModels.RecoveryInfo;

    public class RecoveryInfoController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public RecoveryInfoController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> RecoveryPercentage(Guid id)
        {
            using (var client = apiClient())
            {
                var recoveryPercentageData =
                    await client.SendAsync(User.GetAccessToken(), new GetRecoveryPercentageData(id));

                var model = new RecoveryPercentageViewModel(recoveryPercentageData);

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecoveryPercentage(RecoveryPercentageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await
                        client.SendAsync(User.GetAccessToken(), model.ToRequest());

                    if (model.IsProvidedByImporter)
                    {
                        return RedirectToAction("Index", "Home", new { id = model.NotificationId});
                    }

                    return RedirectToAction("RecoveryValues", "RecoveryInfo", new { isDisposal = !model.IsHundredPercentRecoverable.GetValueOrDefault() });
                }
                catch (ApiBadRequestException e)
                {
                    this.HandleBadRequest(e);
                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }

                return View(model);
            }
        }

        [HttpGet]
        public ActionResult RecoveryValues(Guid id, bool isDisposal)
        {
            var model = new RecoveryInfoValuesViewModel
            {
                NotificationId = id,
                IsDisposal = isDisposal
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecoveryValues(RecoveryInfoValuesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("RecoveryValues", model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(), model.ToRequest());
                    return RedirectToAction("Index", "Home");
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);
                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
                return View("RecoveryValues", model);
            }
        }
    }
}