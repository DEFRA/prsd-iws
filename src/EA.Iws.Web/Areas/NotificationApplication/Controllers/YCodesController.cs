namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.WasteCodes;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.WasteCodes;
    using ViewModels.WasteCodes;

    [Authorize]
    public class YCodesController : Controller
    {
        private readonly Func<IIwsClient> apiClient;
        private static readonly IList<CodeType> RequiredCodeTypes = new[] { CodeType.Y };

        public YCodesController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> EnterYCodes(Guid id)
        {
            var model = new YCodesViewModel();

            using (var client = apiClient())
            {
                var result =
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new GetWasteCodeLookupAndNotificationDataByTypes(id, RequiredCodeTypes, RequiredCodeTypes));

                model.EnterWasteCodesViewModel.Codes = result.LookupWasteCodeData.Values.Single().ToList();
                model.EnterWasteCodesViewModel.SelectedCodesList = result.NotificationWasteCodeData.Values.Single().ToList();
                model.EnterWasteCodesViewModel.NotificationId = id;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnterYCodes(YCodesViewModel model, string command, string remove)
        {
            if (remove != null)
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model.EnterWasteCodesViewModel.NotificationId, model);
                ModelState.Clear();

                if (model.EnterWasteCodesViewModel.SelectedCodesList.Any(c => c.Id.ToString() == remove))
                {
                    model.EnterWasteCodesViewModel.SelectedCodesList.RemoveAll(c => c.Id.ToString() == remove);
                }

                return View(model);
            }

            if (command.Equals("addcode"))
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model.EnterWasteCodesViewModel.NotificationId, model);
                WasteCodeData codeToAdd;

                try
                {
                    codeToAdd = model.EnterWasteCodesViewModel.Codes.Single(c => c.Id.ToString() == model.EnterWasteCodesViewModel.SelectedCode);
                }
                catch (Exception)
                {
                    return View(model);
                }

                if (codeToAdd.Code.Equals("Not applicable"))
                {
                    model.EnterWasteCodesViewModel.SelectedCodesList.Clear();
                    model.EnterWasteCodesViewModel.SelectedCodesList.Add(codeToAdd);
                    return View(model);
                }

                if (model.EnterWasteCodesViewModel.SelectedCodesList == null)
                {
                    model.EnterWasteCodesViewModel.SelectedCodesList = new List<WasteCodeData>();
                }

                if (model.EnterWasteCodesViewModel.SelectedCodesList.All(c => c.Id != codeToAdd.Id))
                {
                    if (model.EnterWasteCodesViewModel.SelectedCodesList.Any(c => c.Code == "Not applicable"))
                    {
                        model.EnterWasteCodesViewModel.SelectedCodesList.RemoveAll(c => c.Code.Equals("Not applicable"));
                    }

                    model.EnterWasteCodesViewModel.SelectedCodesList.Add(codeToAdd);
                }

                return View(model);
            }

            if (!ModelState.IsValid)
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model.EnterWasteCodesViewModel.NotificationId, model);
                return View(model);
            }

            if (command == "continue")
            {
                if (model.EnterWasteCodesViewModel.SelectedCode != null)
                {
                    var selectedYWasteCode = new WasteCodeData { Id = new Guid(model.EnterWasteCodesViewModel.SelectedCode) };
                    if (model.EnterWasteCodesViewModel.SelectedCodesList == null)
                    {
                        model.EnterWasteCodesViewModel.SelectedCodesList = new List<WasteCodeData>();
                        model.EnterWasteCodesViewModel.SelectedCodesList.Add(selectedYWasteCode);
                    }
                    else
                    {
                        if (model.EnterWasteCodesViewModel.SelectedCodesList.All(x => x.Id != selectedYWasteCode.Id))
                        {
                            model.EnterWasteCodesViewModel.SelectedCodesList.Add(selectedYWasteCode);
                        }
                    }
                }

                try
                {
                    return RedirectToAction("EnterYCodes", new { id = model.EnterWasteCodesViewModel.NotificationId });
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

            await InitializeYcodeHcodeAndUnClassViewModel(model.EnterWasteCodesViewModel.NotificationId, model);
            return View(model);
        }

        private async Task<YCodesViewModel> InitializeYcodeHcodeAndUnClassViewModel(Guid id, YCodesViewModel model)
        {
            using (var client = apiClient())
            {
                var result =
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new GetWasteCodeLookupAndNotificationDataByTypes(id, RequiredCodeTypes, RequiredCodeTypes));

                model.EnterWasteCodesViewModel.Codes = result.LookupWasteCodeData.Values.Single().ToList();
                return model;
            }
        }
    }
}