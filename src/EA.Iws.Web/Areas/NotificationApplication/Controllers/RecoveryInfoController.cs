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
        public async Task<ActionResult> AddRecoveryPercentage(Guid id)
        {
            return await GetRecoveryPercentageScreen(id);
        }

        private async Task<ActionResult> GetRecoveryPercentageScreen(Guid id)
        {
            var model = new RecoveryPercentageViewModel();
            model.NotificationId = id;

            using (var client = apiClient())
            {
                var recoveryPercentageData =
                    await client.SendAsync(User.GetAccessToken(), new GetRecoveryPercentageData(id));

                if (recoveryPercentageData.IsProvidedByImporter == null ||
                        recoveryPercentageData.IsProvidedByImporter == false)
                {
                    model.IsProvidedByImporter = false;
                }
                else
                {
                    model.IsProvidedByImporter = true;
                }

                if (recoveryPercentageData.PercentageRecoverable != null)
                {
                    model.PercentageRecoverable = recoveryPercentageData.PercentageRecoverable;
                    model.MethodOfDisposal = recoveryPercentageData.MethodOfDisposal;

                    if (recoveryPercentageData.PercentageRecoverable == Convert.ToDecimal(100.00))
                    {
                        model.IsHundredPercentRecoverable = true;
                    }
                    else
                    {
                        model.IsHundredPercentRecoverable = false;
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRecoveryPercentage(RecoveryPercentageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    var percentage = model.PercentageRecoverable;
                    var isHundredPercentRecoverable = model.IsHundredPercentRecoverable;

                    if (!isHundredPercentRecoverable.HasValue)
                    {
                        isHundredPercentRecoverable = false;
                    }

                    if ((bool)isHundredPercentRecoverable)
                    {
                        percentage = Convert.ToDecimal(100.00);
                    }

                    if (model.IsProvidedByImporter)
                    {
                        percentage = null;
                        model.MethodOfDisposal = null;
                    }

                    await
                        client.SendAsync(User.GetAccessToken(),
                            new SetRecoveryPercentageData(model.NotificationId, model.IsProvidedByImporter,
                                model.MethodOfDisposal, percentage));

                    return RedirectToAction("Index", "Home");
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
        public async Task<ActionResult> EditRecoveryPercentage(Guid id)
        {
            return await GetRecoveryPercentageScreen(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRecoveryPercentage(RecoveryPercentageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    var percentage = model.PercentageRecoverable;
                    var isHundredPercentRecoverable = model.IsHundredPercentRecoverable;

                    if (!isHundredPercentRecoverable.HasValue)
                    {
                        isHundredPercentRecoverable = false;
                    }

                    if ((bool)isHundredPercentRecoverable)
                    {
                        percentage = Convert.ToDecimal(100.00);
                    }

                    if (model.IsProvidedByImporter)
                    {
                        percentage = null;
                        model.MethodOfDisposal = null;
                    }

                    await
                        client.SendAsync(User.GetAccessToken(),
                            new SetRecoveryPercentageData(model.NotificationId, model.IsProvidedByImporter,
                                model.MethodOfDisposal, percentage));

                    return RedirectToAction("Index", "Home");
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
    }
}