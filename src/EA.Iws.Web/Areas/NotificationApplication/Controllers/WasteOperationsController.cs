namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.OperationCodes;
    using Core.Shared;
    using Infrastructure;
    using Prsd.Core.Helpers;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using Requests.OperationCodes;
    using Requests.Shared;
    using Requests.TechnologyEmployed;
    using ViewModels.WasteOperations;
    using Web.ViewModels.Shared;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class WasteOperationsController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public WasteOperationsController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> OperationCodes(Guid id, bool? backToOverview = null)
        {
            using (var client = apiClient())
            {
                try
                {
                    var notificationInfo =
                        await client.SendAsync(User.GetAccessToken(), new GetNotificationBasicInfo(id));

                    if (notificationInfo.NotificationType == NotificationType.Disposal)
                    {
                        return RedirectToAction("DisposalCodes", "WasteOperations", new { id, backToOverview });
                    }

                    if (notificationInfo.NotificationType == NotificationType.Recovery)
                    {
                        return RedirectToAction("RecoveryCodes", "WasteOperations", new { id, backToOverview });
                    }
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);

                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
            }

            throw new InvalidOperationException(
                "Invalid Notification Type. The Notification must be marked as for 'Recovery' or 'Disposal'");
        }

        [HttpGet]
        public async Task<ActionResult> RecoveryCodes(Guid id, bool? backToOverview = null)
        {
            var model = new OperationCodesViewModel { NotificationId = id };
            model.Codes = CheckBoxCollectionViewModel.CreateFromEnum<RecoveryCode>();
            model.CodeInformation = new Dictionary<string, string>();

            foreach (RecoveryCode code in Enum.GetValues(typeof(RecoveryCode)))
            {
                model.CodeInformation.Add(code.ToString(), EnumHelper.GetDescription(code));
            }

            using (var client = apiClient())
            {
                var selectedCodes =
                        await client.SendAsync(User.GetAccessToken(), new GetOperationCodesByNotificationId(id));

                model.Codes.SetSelectedValues(selectedCodes.Select(s => s.Value));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecoveryCodes(OperationCodesViewModel model, bool? backToOverview = null)
        {
            model.CodeInformation = new Dictionary<string, string>();

            foreach (RecoveryCode code in Enum.GetValues(typeof(RecoveryCode)))
            {
                model.CodeInformation.Add(code.ToString(), EnumHelper.GetDescription(code));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    var selectedRecoveryCodes = model.Codes.PossibleValues.Where(r => r.Selected)
                        .Select(r => (RecoveryCode)(Convert.ToInt32(r.Value)))
                        .ToList();

                    if (!selectedRecoveryCodes.Any())
                    {
                        ModelState.AddModelError(string.Empty, "Please select at least one option");
                        return View(model);
                    }

                    await
                        client.SendAsync(User.GetAccessToken(),
                            new AddRecoveryCodes(selectedRecoveryCodes, model.NotificationId));

                    if (backToOverview.GetValueOrDefault())
                    {
                        return RedirectToAction("Index", "Home", new { id = model.NotificationId });
                    }
                    else
                    {
                        return RedirectToAction("TechnologyEmployed", "WasteOperations", new { id = model.NotificationId });
                    }
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
        public async Task<ActionResult> DisposalCodes(Guid id, bool? backToOverview = null)
        {
            var model = new OperationCodesViewModel { NotificationId = id };
            model.Codes = CheckBoxCollectionViewModel.CreateFromEnum<DisposalCode>();
            model.CodeInformation = new Dictionary<string, string>();

            foreach (DisposalCode code in Enum.GetValues(typeof(DisposalCode)))
            {
                model.CodeInformation.Add(code.ToString(), EnumHelper.GetDescription(code));
            }

            using (var client = apiClient())
            {
                var selectedCodes =
                        await client.SendAsync(User.GetAccessToken(), new GetOperationCodesByNotificationId(id));

                model.Codes.SetSelectedValues(selectedCodes.Select(s => s.Value));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisposalCodes(OperationCodesViewModel model, bool? backToOverview = null)
        {
            model.CodeInformation = new Dictionary<string, string>();

            foreach (DisposalCode code in Enum.GetValues(typeof(DisposalCode)))
            {
                model.CodeInformation.Add(code.ToString(), EnumHelper.GetDescription(code));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    var selectedDisposalCodes = model.Codes.PossibleValues.Where(r => r.Selected)
                        .Select(r => (DisposalCode)(Convert.ToInt32(r.Value)))
                        .ToList();

                    if (!selectedDisposalCodes.Any())
                    {
                        ModelState.AddModelError(string.Empty, "Please select at least one option");
                        return View(model);
                    }

                    await
                        client.SendAsync(User.GetAccessToken(),
                            new AddDisposalCodes(selectedDisposalCodes, model.NotificationId));

                    if (backToOverview.GetValueOrDefault())
                    {
                        return RedirectToAction("Index", "Home", new { id = model.NotificationId });
                    }
                    else
                    {
                        return RedirectToAction("TechnologyEmployed", "WasteOperations", new { id = model.NotificationId });
                    }
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
        public async Task<ActionResult> TechnologyEmployed(Guid id, bool? backToOverview = null)
        {
            using (var client = apiClient())
            {
                var model = new TechnologyEmployedViewModel();
                model.NotificationId = id;
                model.OperationCodes = await GetOperationCodes(id, client);

                var technologyEmployedData =
                    await client.SendAsync(User.GetAccessToken(), new GetTechnologyEmployed(id));

                if (technologyEmployedData.HasTechnologyEmployed)
                {
                    model.AnnexProvided = technologyEmployedData.AnnexProvided;
                    model.Details = technologyEmployedData.Details;
                    model.FurtherDetails = technologyEmployedData.FurtherDetails;
                }

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TechnologyEmployed(TechnologyEmployedViewModel model, bool? backToOverview = null)
        {
            using (var client = apiClient())
            {
                if (!ModelState.IsValid)
                {
                    model.OperationCodes = await GetOperationCodes(model.NotificationId, client);
                    return View(model);
                }
                try
                {
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new SetTechnologyEmployed(model.NotificationId, model.AnnexProvided, model.Details, model.FurtherDetails));

                    if (backToOverview.GetValueOrDefault())
                    {
                        return RedirectToAction("Index", "Home",
                        new { id = model.NotificationId });
                    }
                    else
                    {
                        return RedirectToAction("Index", "ReasonForExport",
                                    new { id = model.NotificationId }); 
                    }
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

        private async Task<List<string>> GetOperationCodes(Guid id, IIwsClient client)
        {
            var codeDatas =
                await client.SendAsync(User.GetAccessToken(), new GetOperationCodesByNotificationId(id));

            var orderedCodeDatas = codeDatas.OrderBy(c => c.Value);

            return orderedCodeDatas.Select(c => c.Code).ToList();
        }
    }
}