namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using ViewModels.SpecialHandling;

    [Authorize]
    public class SpecialHandlingController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public SpecialHandlingController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            var model = new SpecialHandlingViewModel { NotificationId = id };
            using (var client = apiClient())
            {
                var specialHandlingData =
                    await client.SendAsync(User.GetAccessToken(), new GetSpecialHandingForNotification(id));

                if (specialHandlingData.HasSpecialHandlingRequirements.HasValue)
                {
                    model.HasSpecialHandlingRequirements = specialHandlingData.HasSpecialHandlingRequirements;
                    model.SpecialHandlingDetails = specialHandlingData.SpecialHandlingDetails;
                    return RedirectToAction("Edit", new { id });
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(SpecialHandlingViewModel model)
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
                    client.SendAsync(User.GetAccessToken(),
                        new SetSpecialHandling(model.NotificationId, model.HasSpecialHandlingRequirements.GetValueOrDefault(),
                            model.SpecialHandlingDetails));

                    return RedirectToAction("Add", "StateOfExport", new { id = model.NotificationId });
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
        public async Task<ActionResult> Edit(Guid id)
        {
            var model = new SpecialHandlingViewModel { NotificationId = id };
            using (var client = apiClient())
            {
                var specialHandlingData =
                    await client.SendAsync(User.GetAccessToken(), new GetSpecialHandingForNotification(id));

                if (!specialHandlingData.HasSpecialHandlingRequirements.HasValue)
                {
                    return RedirectToAction("Add", new { id });
                }

                model.HasSpecialHandlingRequirements = specialHandlingData.HasSpecialHandlingRequirements;
                model.SpecialHandlingDetails = specialHandlingData.SpecialHandlingDetails;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SpecialHandlingViewModel model)
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
                    client.SendAsync(User.GetAccessToken(),
                        new SetSpecialHandling(model.NotificationId, model.HasSpecialHandlingRequirements.GetValueOrDefault(),
                            model.SpecialHandlingDetails));

                    return RedirectToAction("NotificationOverview", "NotificationApplication", new { id = model.NotificationId });
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
    }
}