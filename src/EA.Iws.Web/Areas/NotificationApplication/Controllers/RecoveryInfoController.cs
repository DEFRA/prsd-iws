namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.RecoveryInfo;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using Requests.RecoveryInfo;
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
                var recoveryPercentageData = await client.SendAsync(User.GetAccessToken(), new GetRecoveryPercentageData(id));

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
                    if (model.IsProvidedByImporter)
                    {
                        await client.SendAsync(User.GetAccessToken(), new SetRecoveryPercentageData(model.NotificationId, true, null, null));
                        return RedirectToAction("Index", "Home", new { id = model.NotificationId });
                    }

                    if (model.PercentageRecoverable.HasValue && model.PercentageRecoverable.Value == 100.00M)
                    {
                        await client.SendAsync(User.GetAccessToken(), model.ToRequest());
                        return RedirectToAction("RecoveryValues", "RecoveryInfo", new { isDisposal = false });
                    }

                    return RedirectToAction("MethodOfDisposal", "RecoveryInfo", new { id = model.NotificationId, recoveryPercentage = model.PercentageRecoverable.GetValueOrDefault() });
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
        public ActionResult MethodOfDisposal(Guid id, decimal recoveryPercentage)
        {
            var model = new MethodOfDisposalViewModel(id, recoveryPercentage);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MethodOfDisposal(MethodOfDisposalViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(), model.ToRequest());
                    return RedirectToAction("RecoveryValues", "RecoveryInfo", new { isDisposal = true });
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
        public async Task<ActionResult> RecoveryValues(Guid id, bool isDisposal)
        {
            using (var client = apiClient())
            {
                var recoveryValuesData = await client.SendAsync(User.GetAccessToken(), new GetRecoveryValuesData(id)) ?? new RecoveryInfoData();

                var model = new RecoveryInfoValuesViewModel(recoveryValuesData);

                model.NotificationId = id;
                model.IsDisposal = isDisposal;

                return View(model);
            }
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
                    if (!model.IsDisposal)
                    {
                        model.DisposalAmount = null;
                        model.DisposalUnit = null;
                    }

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