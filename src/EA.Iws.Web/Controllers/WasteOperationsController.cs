namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Helpers;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using Requests.OperationCodes;
    using Requests.Shared;
    using ViewModels.Shared;
    using ViewModels.WasteOperations;

    public class WasteOperationsController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public WasteOperationsController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> OperationCodes(Guid id)
        {
            using (var client = apiClient())
            {
                try
                { 
                    var response = await client.SendAsync(User.GetAccessToken(), new GetNotificationInfo(id));
                    
                    if (response.NotificationType == NotificationType.Disposal)
                    {
                        return RedirectToAction("AddDisposalCodes", "WasteOperations", new { id });
                    }

                    if (response.NotificationType == NotificationType.Recovery)
                    {
                        return RedirectToAction("AddRecoveryCodes", "WasteOperations", new { id });
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
            
            throw new InvalidOperationException("Invalid Notification Type. The Notification must be marked as for 'Recovery' or 'Disposal'");
        }

        [HttpGet]
        public ActionResult AddRecoveryCodes(Guid id)
        {
            var model = new OperationCodesViewModel {NotificationId = id};
            model.Codes = CheckBoxCollectionViewModel.CreateFromEnum<RecoveryCode>();
            model.CodeInformation = new Dictionary<string, string>();

            foreach (RecoveryCode code in Enum.GetValues(typeof(RecoveryCode)))
            {
                model.CodeInformation.Add(code.ToString(), EnumHelper.GetDescription(code));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRecoveryCodes(OperationCodesViewModel model)
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

                    return RedirectToAction("ReasonForExport", "NotificationApplication", new { id = model.NotificationId });
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
        public ActionResult AddDisposalCodes(Guid id)
        {
            var model = new OperationCodesViewModel { NotificationId = id };
            model.Codes = CheckBoxCollectionViewModel.CreateFromEnum<DisposalCode>();
            model.CodeInformation = new Dictionary<string, string>();

            foreach (DisposalCode code in Enum.GetValues(typeof(DisposalCode)))
            {
                model.CodeInformation.Add(code.ToString(), EnumHelper.GetDescription(code));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddDisposalCodes(OperationCodesViewModel model)
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

                    return RedirectToAction("Add", "Carrier", new { id = model.NotificationId });
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