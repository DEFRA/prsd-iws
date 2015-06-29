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

    public class WasteCodesController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public WasteCodesController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> WasteCode(Guid id)
        {
            var model = new WasteCodeViewModel
            {
                NotificationId = id
            };

            await InitializeWasteCodeViewModel(model);

            using (var client = apiClient())
            {
                model.SelectedEwcCodes = new List<WasteCodeData>(await client.SendAsync(User.GetAccessToken(), new GetWasteCodesForNotification(model.NotificationId, CodeType.Ewc)));
                var baselOecdCode = (await client.SendAsync(User.GetAccessToken(), new GetWasteCodesForNotification(model.NotificationId, CodeType.Basel))).SingleOrDefault();
                model.SelectedWasteCode = baselOecdCode == null ? null : baselOecdCode.Id.ToString();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WasteCode(WasteCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await InitializeWasteCodeViewModel(model);
                return View(model);
            }

            if (model.Command.Equals("add"))
            {
                await InitializeWasteCodeViewModel(model);
                var codeToAdd = model.EwcCodes.Single(c => c.Id.ToString() == model.SelectedEwcCode);
                if (model.SelectedEwcCodes.All(c => c.Id != codeToAdd.Id))
                {
                    model.SelectedEwcCodes.Add(codeToAdd);
                }
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new SetWasteCodes(model.NotificationId, new Guid(model.SelectedWasteCode), model.SelectedEwcCodes.Select(p => p.Id)));

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
                await InitializeWasteCodeViewModel(model);
                return View(model);
            }
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

                var importCode = (await client.SendAsync(User.GetAccessToken(), new GetWasteCodesForNotification(model.NotificationId, CodeType.ImportCode))).SingleOrDefault();
                model.ImportNationalCode = importCode == null ? null : importCode.CustomCode;

                var otherCode = (await client.SendAsync(User.GetAccessToken(), new GetWasteCodesForNotification(model.NotificationId, CodeType.OtherCode))).SingleOrDefault();
                model.OtherCode = otherCode == null ? null : otherCode.CustomCode;
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
                            new SetOtherWasteCodes(model.NotificationId, model.ExportNationalCode, model.ImportNationalCode, model.OtherCode));

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
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UnNumber(UnNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await InitializeUnNumberViewModel(model);
                return View(model);
            }

            if (model.Command.Equals("add"))
            {
                await InitializeUnNumberViewModel(model);
                var codeToAdd = model.UnCodes.Single(c => c.Id.ToString() == model.SelectedUnCode);
                if (model.SelectedUnCodes.All(c => c.Id != codeToAdd.Id))
                {
                    model.SelectedUnCodes.Add(codeToAdd);
                }
                return View(model);
            }

            if (model.Command.Equals("addCustomCode"))
            {
                await InitializeUnNumberViewModel(model);
                if (model.CustomCodes == null)
                {
                    model.CustomCodes = new List<string>();
                }
                model.CustomCodes.Add(model.SelectedCustomCode);
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new SetUnNumberWasteCodes(model.NotificationId, model.SelectedUnCodes.Select(p => p.Id), model.CustomCodes));

                    return RedirectToAction("RecoveryPercentage", "RecoveryInfo");
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
        public async Task<ActionResult> AddYcodeHcodeAndUnClass(YcodeHcodeAndUnClassViewModel model, string command)
        {
            //Y codes - non js
            if (command.Equals("addYcode"))
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model);
                var codeToAdd = model.Ycodes.Single(c => c.Id.ToString() == model.SelectedYcode);
                if (model.SelectedYcodesList.All(c => c.Id != codeToAdd.Id))
                {
                    model.SelectedYcodesList.Add(codeToAdd);
                }
                return View(model);
            }

            //H codes - non js
            if (command.Equals("addHcode"))
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model);
                var codeToAdd = model.Hcodes.Single(c => c.Id.ToString() == model.SelectedHcode);
                if (model.SelectedHcodesList.All(c => c.Id != codeToAdd.Id))
                {
                    model.SelectedHcodesList.Add(codeToAdd);
                }
                return View(model);
            }

            //UN classes - non js
            if (command.Equals("addUnClass"))
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model);
                var codeToAdd = model.UnClasses.Single(c => c.Id.ToString() == model.SelectedUnClass);
                if (model.SelectedUnClassesList.All(c => c.Id != codeToAdd.Id))
                {
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

        private async Task<WasteCodeViewModel> InitializeWasteCodeViewModel(WasteCodeViewModel model)
        {
            using (var client = apiClient())
            {
                model.EwcCodes = await client.SendAsync(User.GetAccessToken(), new GetWasteCodesByType(CodeType.Ewc));
                var baselCodes = await client.SendAsync(User.GetAccessToken(), new GetWasteCodesByType(CodeType.Basel));
                var oecdCodes = await client.SendAsync(User.GetAccessToken(), new GetWasteCodesByType(CodeType.Oecd));

                model.WasteCodes = baselCodes.Union(oecdCodes);

                return model;
            }
        }

        private async Task<UnNumberViewModel> InitializeUnNumberViewModel(UnNumberViewModel model)
        {
            using (var client = apiClient())
            {
                model.UnCodes = await client.SendAsync(User.GetAccessToken(), new GetWasteCodesByType(CodeType.UnNumber));

                model.SelectedUnCodes = new List<WasteCodeData>(await client.SendAsync(User.GetAccessToken(), new GetWasteCodesForNotification(model.NotificationId, CodeType.UnNumber)));
                model.CustomCodes = new List<string>((await client.SendAsync(User.GetAccessToken(), new GetWasteCodesForNotification(model.NotificationId, CodeType.CustomsCode))).Select(p => p.CustomCode));

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