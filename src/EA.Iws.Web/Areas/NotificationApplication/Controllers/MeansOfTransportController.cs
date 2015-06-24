namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.MeansOfTransport;
    using Infrastructure;
    using Prsd.Core.Domain;
    using Prsd.Core.Web.ApiClient;
    using Requests.MeansOfTransport;
    using ViewModels.MeansOfTransport;
    using Web.ViewModels.Shared;

    [Authorize]
    public class MeansOfTransportController : Controller
    {
        private readonly Func<IIwsClient> apiClient;
        private const string AddAction = "ADD";
        private const string SubmitAction = "SUBMIT";

        public MeansOfTransportController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            IList<MeansOfTransport> currentMeans;
            MeansOfTransportViewModel model;

            using (var client = apiClient())
            {
                currentMeans = await client.SendAsync(User.GetAccessToken(), new GetMeansOfTransportByNotificationId(id));

                if (currentMeans.Count == 0)
                {
                    model = new MeansOfTransportViewModel
                    {
                        PossibleMeans = GetPossibleMeans()
                    };

                    return View(model);
                }
            }

            var selectedItem = currentMeans[currentMeans.Count - 1];
            currentMeans.RemoveAt(currentMeans.Count - 1);

            model = new MeansOfTransportViewModel
            {
                PossibleMeans = GetPossibleMeans(),
                SelectedMeans = currentMeans.Select(m => m.Value).ToList(),
                SelectedValue = selectedItem.Value
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, MeansOfTransportViewModel model, string submit)
        {
            if (!ModelState.IsValid || submit == null || !model.SelectedValue.HasValue)
            {
                return View(model);
            }

            switch (submit.ToUpperInvariant())
            {
                case AddAction:
                    model.SelectedMeans.Add(model.SelectedValue.Value);
                    model.SelectedValue = null;
                    return View(model);
                case SubmitAction:
                    return await SubmitControllerAction(id, model);
                default:
                    return View(model);
            }
        }

        public ActionResult _CurrentMeansEditor(MeansOfTransportViewModel model)
        {
            this.RemoveModelStateErrors();
            return PartialView(model);
        }

        public ActionResult _PreviousMeansEditor(MeansOfTransportViewModel model)
        {
            return PartialView(model);
        }

        private IList<RadioButtonPair<string, int>> GetPossibleMeans()
        {
            return Enumeration.GetAll<MeansOfTransport>()
                .Select(mot => new RadioButtonPair<string, int>(mot.DisplayName, mot.Value))
                .ToArray();
        }

        private async Task<ActionResult> SubmitControllerAction(Guid id, MeansOfTransportViewModel model)
        {
            try
            {
                using (var client = apiClient())
                {
                    model.SelectedMeans.Add(model.SelectedValue.Value);

                    var meansList = model.SelectedMeans.Select(Enumeration.FromValue<MeansOfTransport>).ToArray();

                    var result = await client.SendAsync(User.GetAccessToken(), new SetMeansOfTransportForNotification(id, meansList));
                }
            }
            catch (ApiException)
            {
                ModelState.AddModelError(string.Empty, "An error occurred saving this record");
                model.SelectedMeans.RemoveAt(model.SelectedMeans.Count - 1);
                return View(model);
            }

            return RedirectToAction("Index", "PackagingTypes", new { id });
        }
    }
}