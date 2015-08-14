namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.Shared;
    using Core.WasteCodes;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using Requests.WasteCodes;
    using ViewModels.WasteCodes;

    public class WasteCodesController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public WasteCodesController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> BaselEwcCode(Guid id)
        {
            var model = new BaselEwcCodeViewModel
            {
                NotificationId = id
            };

            await InitializeBaselEwcCodeViewModel(model);

            using (var client = apiClient())
            {
                model.SelectedEwcCodes = new List<WasteCodeData>(await client.SendAsync(User.GetAccessToken(), new GetWasteCodesForNotification(model.NotificationId, CodeType.Ewc)));
                var baselOecdCode = (await client.SendAsync(User.GetAccessToken(), new GetWasteCodesForNotification(model.NotificationId, CodeType.Basel))).SingleOrDefault();
                model.SelectedBaselCode = baselOecdCode == null ? null : baselOecdCode.Id.ToString();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BaselEwcCode(BaselEwcCodeViewModel model, string command, string remove)
        {
            if (remove != null)
            {
                await InitializeBaselEwcCodeViewModel(model);
                ModelState.Clear();

                if (model.SelectedEwcCodes.Any(c => c.Id.ToString() == remove))
                {
                    model.SelectedEwcCodes.RemoveAll(c => c.Id.ToString() == remove);
                }

                return View(model);
            }

            if (command.Equals("addEwcCode"))
            {
                await InitializeBaselEwcCodeViewModel(model);
                ModelState.Clear();
                model.TypeOfCodeAdded = "EWC";
                WasteCodeData codeToAdd;

                try
                {
                    codeToAdd = model.EwcCodes.Single(c => c.Id.ToString() == model.SelectedEwcCode);
                }
                catch (Exception)
                {
                    return View(model);
                }

                if (model.SelectedEwcCodes.All(c => c.Id != codeToAdd.Id))
                {
                    model.SelectedEwcCodes.Add(codeToAdd);
                }

                return View(model);
            }

            if (!ModelState.IsValid)
            {
                await InitializeBaselEwcCodeViewModel(model);
                return View(model);
            }

            if (command.Equals("continue"))
            {
                using (var client = apiClient())
                {
                    if (model.SelectedEwcCode != null)
                    {
                        var selectedEwcWasteCode = new WasteCodeData { Id = new Guid(model.SelectedEwcCode) };
                        if (model.SelectedEwcCodes == null)
                        {
                            model.SelectedEwcCodes = new List<WasteCodeData>();
                            model.SelectedEwcCodes.Add(selectedEwcWasteCode);
                        }
                        else
                        {
                            if (model.SelectedEwcCodes.All(x => x.Id != selectedEwcWasteCode.Id))
                            {
                                model.SelectedEwcCodes.Add(selectedEwcWasteCode);
                            }
                        }
                    }

                    try
                    {
                        await
                            client.SendAsync(User.GetAccessToken(),
                                new SetWasteCodes(model.NotificationId, new Guid(model.SelectedBaselCode), model.SelectedEwcCodes.Select(p => p.Id)));

                        return RedirectToAction("OtherWasteCodes", new { id = model.NotificationId });
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
            }

            await InitializeBaselEwcCodeViewModel(model);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> OtherWasteCodes(Guid id)
        {
            var model = new OtherWasteCodesViewModel
            {
                NotificationId = id
            };

            using (var client = apiClient())
            {
                var exportCode = (await client.SendAsync(User.GetAccessToken(), new GetWasteCodesForNotification(model.NotificationId, CodeType.ExportCode))).SingleOrDefault();
                model.ExportNationalCode = exportCode == null ? null : exportCode.CustomCode;
                model.ExportNationalCodeNotApplicable = exportCode != null && exportCode.IsNotApplicable;

                var importCode = (await client.SendAsync(User.GetAccessToken(), new GetWasteCodesForNotification(model.NotificationId, CodeType.ImportCode))).SingleOrDefault();
                model.ImportNationalCode = importCode == null ? null : importCode.CustomCode;
                model.ImportNationalCodeNotApplicable = importCode != null && importCode.IsNotApplicable;

                var customsCode = (await client.SendAsync(User.GetAccessToken(), new GetWasteCodesForNotification(model.NotificationId, CodeType.CustomsCode))).SingleOrDefault();
                model.CustomsCode = customsCode == null ? null : customsCode.CustomCode;
                model.CustomsCodeNotApplicable = customsCode != null && customsCode.IsNotApplicable;

                var otherCode = (await client.SendAsync(User.GetAccessToken(), new GetWasteCodesForNotification(model.NotificationId, CodeType.OtherCode))).SingleOrDefault();
                model.OtherCode = otherCode == null ? null : otherCode.CustomCode;
                model.OtherCodeNotApplicable = otherCode != null && otherCode.IsNotApplicable;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OtherWasteCodes(OtherWasteCodesViewModel model)
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
                            new SetOtherWasteCodes(model.NotificationId, model.ExportNationalCode, model.ImportNationalCode, model.CustomsCode, model.OtherCode));

                    return RedirectToAction("AddYcodeHcodeAndUnClass", new { id = model.NotificationId });
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
        public async Task<ActionResult> UnNumber(Guid id)
        {
            var model = new UnNumberViewModel
            {
                NotificationId = id
            };
            await InitializeUnNumberViewModel(model);

            using (var client = apiClient())
            {
                model.SelectedUnNumbers =
                    new List<WasteCodeData>(
                        await
                            client.SendAsync(User.GetAccessToken(),
                                new GetWasteCodesForNotification(model.NotificationId, CodeType.UnNumber)));
                model.SelectedCustomCodes =
                    new List<string>(
                        (await
                            client.SendAsync(User.GetAccessToken(),
                                new GetWasteCodesForNotification(model.NotificationId, CodeType.CustomsCode))).Select(
                                    p => p.CustomCode));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UnNumber(UnNumberViewModel model, string command, string remove)
        {
            if (remove != null)
            {
                await InitializeUnNumberViewModel(model);
                ModelState.Clear();

                if (model.SelectedUnNumbers.Any(c => c.Id.ToString() == remove))
                {
                    model.SelectedUnNumbers.RemoveAll(c => c.Id.ToString() == remove);
                }

                if (model.SelectedCustomCodes.Any(c => c == remove))
                {
                    model.SelectedCustomCodes.RemoveAll(c => c == remove);
                }

                return View(model);
            }

            if (command.Equals("addUnNumber"))
            {
                await InitializeUnNumberViewModel(model);
                ModelState.Clear();
                model.TypeOfCodeAdded = "UN";
                WasteCodeData codeToAdd;

                try
                {
                    codeToAdd = model.UnNumbers.Single(c => c.Id.ToString() == model.SelectedUnNumber);
                }
                catch (Exception)
                {
                    return View(model);
                }

                if (model.SelectedUnNumbers.All(c => c.Id != codeToAdd.Id))
                {
                    model.SelectedUnNumbers.Add(codeToAdd);
                }

                if (codeToAdd.Code.Equals("Not applicable"))
                {
                    model.SelectedUnNumbers.Clear();
                    model.SelectedUnNumbers.Add(codeToAdd);
                    return View(model);
                }

                if (model.SelectedUnNumbers.All(c => c.Id != codeToAdd.Id))
                {
                    if (model.SelectedUnNumbers.Any(c => c.Code == "Not applicable"))
                    {
                        model.SelectedUnNumbers.RemoveAll(c => c.Code.Equals("Not applicable"));
                    }

                    model.SelectedUnNumbers.Add(codeToAdd);
                }

                return View(model);
            }

            if (command.Equals("addCustomCode"))
            {
                await InitializeUnNumberViewModel(model);
                ModelState.Clear();
                model.TypeOfCodeAdded = "Custom";

                if (model.SelectedCustomCodes == null)
                {
                    model.SelectedCustomCodes = new List<string>();
                }

                if (!model.SelectedCustomCodes.Contains(model.SelectedCustomCode))
                {
                    model.SelectedCustomCodes.Add(model.SelectedCustomCode);
                }

                return View(model);
            }

            if (!ModelState.IsValid)
            {
                await InitializeUnNumberViewModel(model);
                return View(model);
            }

            using (var client = apiClient())
            {
                if (model.SelectedUnNumber != null)
                {
                    var selectedUnNumber = new WasteCodeData { Id = new Guid(model.SelectedUnNumber) };
                    if (model.SelectedUnNumbers == null)
                    {
                        model.SelectedUnNumbers = new List<WasteCodeData>();
                        model.SelectedUnNumbers.Add(selectedUnNumber);
                    }
                    else
                    {
                        if (model.SelectedUnNumbers.All(x => x.Id != selectedUnNumber.Id))
                        {
                            model.SelectedUnNumbers.Add(selectedUnNumber);
                        }
                    }
                }

                if (model.SelectedCustomCode != null)
                {
                    if (model.SelectedCustomCodes == null)
                    {
                        model.SelectedCustomCodes = new List<string>();
                        model.SelectedCustomCodes.Add(model.SelectedCustomCode);
                    }
                    else
                    {
                        if (!model.SelectedCustomCodes.Contains(model.SelectedCustomCode))
                        {
                            model.SelectedCustomCodes.Add(model.SelectedCustomCode);
                        }
                    }
                }

                try
                {
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new SetUnNumberWasteCodes(model.NotificationId, model.SelectedUnNumbers.Select(p => p.Id), model.SelectedCustomCodes));

                    var response = await client.SendAsync(User.GetAccessToken(), new GetNotificationBasicInfo(model.NotificationId));
                    if (response.NotificationType == NotificationType.Recovery)
                    {
                        return RedirectToAction("RecoveryPercentage", "RecoveryInfo");
                    }
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

                await InitializeUnNumberViewModel(model);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> AddYcodeHcodeAndUnClass(Guid id)
        {
            var model = new YcodeHcodeAndUnClassViewModel
            {
                NotificationId = id
            };

            await InitializeYcodeHcodeAndUnClassViewModel(model);

            using (var client = apiClient())
            {
                model.SelectedYcodesList = new List<WasteCodeData>(await client.SendAsync(User.GetAccessToken(), new GetWasteCodesForNotification(model.NotificationId, CodeType.Y)));
                model.SelectedHcodesList = new List<WasteCodeData>(await client.SendAsync(User.GetAccessToken(), new GetWasteCodesForNotification(model.NotificationId, CodeType.H)));
                model.SelectedUnClassesList = new List<WasteCodeData>(await client.SendAsync(User.GetAccessToken(), new GetWasteCodesForNotification(model.NotificationId, CodeType.Un)));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddYcodeHcodeAndUnClass(YcodeHcodeAndUnClassViewModel model, string command, string remove)
        {
            if (remove != null)
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model);
                ModelState.Clear();

                if (model.SelectedYcodesList.Any(c => c.Id.ToString() == remove))
                {
                    model.SelectedYcodesList.RemoveAll(c => c.Id.ToString() == remove);
                }

                if (model.SelectedHcodesList.Any(c => c.Id.ToString() == remove))
                {
                    model.SelectedHcodesList.RemoveAll(c => c.Id.ToString() == remove);
                }

                if (model.SelectedUnClassesList.Any(c => c.Id.ToString() == remove))
                {
                    model.SelectedUnClassesList.RemoveAll(c => c.Id.ToString() == remove);
                }

                return View(model);
            }

            //Y codes
            if (command.Equals("addYcode"))
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model);
                ModelState.Remove("TypeOfCodeAdded");
                ModelState.Clear();
                model.TypeOfCodeAdded = "Y";
                WasteCodeData codeToAdd;

                try
                {
                    codeToAdd = model.Ycodes.Single(c => c.Id.ToString() == model.SelectedYcode);
                }
                catch (Exception)
                {
                    return View(model);
                }

                if (codeToAdd.Code.Equals("Not applicable"))
                {
                    model.SelectedYcodesList.Clear();
                    model.SelectedYcodesList.Add(codeToAdd);
                    return View(model);
                }

                if (model.SelectedYcodesList.All(c => c.Id != codeToAdd.Id))
                {
                    if (model.SelectedYcodesList.Any(c => c.Code == "Not applicable"))
                    {
                        model.SelectedYcodesList.RemoveAll(c => c.Code.Equals("Not applicable"));
                    }

                    model.SelectedYcodesList.Add(codeToAdd);
                }

                return View(model);
            }

            //H codes
            if (command.Equals("addHcode"))
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model);
                ModelState.Remove("TypeOfCodeAdded");
                ModelState.Clear();
                model.TypeOfCodeAdded = "H";
                WasteCodeData codeToAdd;

                try
                {
                    codeToAdd = model.Hcodes.Single(c => c.Id.ToString() == model.SelectedHcode);
                }
                catch (Exception)
                {
                    return View(model);
                }

                if (codeToAdd.Code.Equals("Not applicable"))
                {
                    model.SelectedHcodesList.Clear();
                    model.SelectedHcodesList.Add(codeToAdd);
                    return View(model);
                }

                if (model.SelectedHcodesList.All(c => c.Id != codeToAdd.Id))
                {
                    if (model.SelectedHcodesList.Any(c => c.Code == "Not applicable"))
                    {
                        model.SelectedHcodesList.RemoveAll(c => c.Code.Equals("Not applicable"));
                    }

                    model.SelectedHcodesList.Add(codeToAdd);
                }

                return View(model);
            }

            //UN classes
            if (command.Equals("addUnClass"))
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model);
                ModelState.Remove("TypeOfCodeAdded");
                ModelState.Clear();
                model.TypeOfCodeAdded = "UN";
                WasteCodeData codeToAdd;

                try
                {
                    codeToAdd = model.UnClasses.Single(c => c.Id.ToString() == model.SelectedUnClass);
                }
                catch (Exception)
                {
                    return View(model);
                }

                if (codeToAdd.Code.Equals("Not applicable"))
                {
                    model.SelectedUnClassesList.Clear();
                    model.SelectedUnClassesList.Add(codeToAdd);
                    return View(model);
                }

                if (model.SelectedUnClassesList.All(c => c.Id != codeToAdd.Id))
                {
                    if (model.SelectedUnClassesList.Any(c => c.Code == "Not applicable"))
                    {
                        model.SelectedUnClassesList.RemoveAll(c => c.Code.Equals("Not applicable"));
                    }

                    model.SelectedUnClassesList.Add(codeToAdd);
                }

                return View(model);
            }

            if (!ModelState.IsValid)
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model);
                return View(model);
            }

            if (command == "continue")
            {
                using (var client = apiClient())
                {
                    if (model.SelectedYcode != null)
                    {
                        var selectedYWasteCode = new WasteCodeData { Id = new Guid(model.SelectedYcode) };
                        if (model.SelectedYcodesList == null)
                        {
                            model.SelectedYcodesList = new List<WasteCodeData>();
                            model.SelectedYcodesList.Add(selectedYWasteCode);
                        }
                        else
                        {
                            if (model.SelectedYcodesList.All(x => x.Id != selectedYWasteCode.Id))
                            {
                                model.SelectedYcodesList.Add(selectedYWasteCode);
                            }
                        }
                    }

                    if (model.SelectedHcode != null)
                    {
                        var selectedHWasteCode = new WasteCodeData { Id = new Guid(model.SelectedHcode) };
                        if (model.SelectedHcodesList == null)
                        {
                            model.SelectedHcodesList = new List<WasteCodeData>();
                            model.SelectedHcodesList.Add(selectedHWasteCode);
                        }
                        else
                        {
                            if (model.SelectedHcodesList.All(x => x.Id != selectedHWasteCode.Id))
                            {
                                model.SelectedHcodesList.Add(selectedHWasteCode);
                            }
                        }
                    }

                    if (model.SelectedUnClass != null)
                    {
                        var selectedUnWasteCode = new WasteCodeData { Id = new Guid(model.SelectedUnClass) };
                        if (model.SelectedUnClassesList == null)
                        {
                            model.SelectedUnClassesList = new List<WasteCodeData>();
                            model.SelectedUnClassesList.Add(selectedUnWasteCode);
                        }
                        else
                        {
                            if (model.SelectedUnClassesList.All(x => x.Id != selectedUnWasteCode.Id))
                            {
                                model.SelectedUnClassesList.Add(selectedUnWasteCode);
                            }
                        }
                    }

                    try
                    {
                        await
                            client.SendAsync(User.GetAccessToken(),
                                new SetYHUnWasteCodes(model.NotificationId, model.SelectedYcodesList.Select(p => p.Id),
                                    model.SelectedHcodesList.Select(p => p.Id), model.SelectedUnClassesList.Select(p => p.Id)));

                        return RedirectToAction("UnNumber", new { id = model.NotificationId });
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
            }

            await InitializeYcodeHcodeAndUnClassViewModel(model);
            return View(model);
        }

        private async Task<BaselEwcCodeViewModel> InitializeBaselEwcCodeViewModel(BaselEwcCodeViewModel model)
        {
            using (var client = apiClient())
            {
                model.EwcCodes = await client.SendAsync(User.GetAccessToken(), new GetWasteCodesByType(CodeType.Ewc));
                var baselCodes = await client.SendAsync(User.GetAccessToken(), new GetWasteCodesByType(CodeType.Basel));
                var oecdCodes = await client.SendAsync(User.GetAccessToken(), new GetWasteCodesByType(CodeType.Oecd));

                model.BaselOecdCodes = baselCodes.Union(oecdCodes).OrderBy(m => m.Code);
                return model;
            }
        }

        private async Task<UnNumberViewModel> InitializeUnNumberViewModel(UnNumberViewModel model)
        {
            using (var client = apiClient())
            {
                model.UnNumbers = await client.SendAsync(User.GetAccessToken(), new GetWasteCodesByType(CodeType.UnNumber));
                return model;
            }
        }

        private async Task<YcodeHcodeAndUnClassViewModel> InitializeYcodeHcodeAndUnClassViewModel(
            YcodeHcodeAndUnClassViewModel model)
        {
            using (var client = apiClient())
            {
                model.Ycodes = await client.SendAsync(User.GetAccessToken(), new GetWasteCodesByType(CodeType.Y));
                model.Hcodes = await client.SendAsync(User.GetAccessToken(), new GetWasteCodesByType(CodeType.H));
                model.UnClasses = await client.SendAsync(User.GetAccessToken(), new GetWasteCodesByType(CodeType.Un));

                return model;
            }
        }
    }
}