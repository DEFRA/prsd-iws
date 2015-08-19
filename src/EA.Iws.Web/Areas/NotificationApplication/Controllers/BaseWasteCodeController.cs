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
    using Requests.WasteCodes;
    using ViewModels.WasteCodes;
    using Views.Shared;

    public abstract class BaseWasteCodeController : Controller
    {
        protected const string AddCode = "addcode";
        protected const string Continue = "continue";
        protected readonly Func<IIwsClient> ApiClient;
        private readonly CodeType codeType;

        protected BaseWasteCodeController(Func<IIwsClient> apiClient, CodeType codeType)
        {
            this.ApiClient = apiClient;
            this.codeType = codeType;
        }

        public async Task<ActionResult> Post(Guid id, BaseWasteCodeViewModel viewModel, string command, string remove)
        {
            await RebindModel(id, viewModel);

            if (!string.IsNullOrWhiteSpace(remove))
            {
                ModelState.Clear();

                viewModel.EnterWasteCodesViewModel.SelectedWasteCodes.RemoveAll(c => c.ToString().Equals(remove));

                return RemoveAction(viewModel);
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            if (command.Equals(AddCode) && HasASelectedCode(viewModel))
            {
                AddCodeToViewModel(viewModel);

                viewModel.EnterWasteCodesViewModel.SelectedCode = null;

                ModelState.Remove("EnterWasteCodesViewModel.SelectedCode");

                return AddAction(viewModel);
            }

            if (command.Equals(Continue))
            {
                if (HasASelectedCode(viewModel))
                {
                    AddCodeToViewModel(viewModel);
                }

                return await ContinueAction(id, viewModel);
            }

            throw new InvalidOperationException();
        }

        private async Task RebindModel(Guid id, BaseWasteCodeViewModel viewModel)
        {
            using (var client = ApiClient())
            {
                var result =
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new GetWasteCodeLookupAndNotificationDataByTypes(id, new[] { codeType }));

                viewModel.EnterWasteCodesViewModel.WasteCodes =
                    result.LookupWasteCodeData[codeType].Select(wc => new WasteCodeViewModel
                    {
                        Id = wc.Id,
                        CodeType = wc.CodeType,
                        Description = wc.Description,
                        Name = wc.Code
                    }).ToList();
            }
        }

        private void AddCodeToViewModel(BaseWasteCodeViewModel viewModel)
        {
            if (viewModel.EnterWasteCodesViewModel.SelectedWasteCodes == null)
            {
                viewModel.EnterWasteCodesViewModel.SelectedWasteCodes = new List<Guid>();
            }

            if (viewModel.EnterWasteCodesViewModel.SelectedWasteCodes
                .Contains(viewModel.EnterWasteCodesViewModel.SelectedCode.Value))
            {
                return;
            }

            viewModel.EnterWasteCodesViewModel.SelectedWasteCodes.Add(viewModel.EnterWasteCodesViewModel.SelectedCode.Value);
        }

        protected virtual ActionResult RemoveAction(BaseWasteCodeViewModel viewModel)
        {
            return View(viewModel);
        }

        protected virtual ActionResult AddAction(BaseWasteCodeViewModel viewModel)
        {
            return View(viewModel);
        }

        private bool HasASelectedCode(BaseWasteCodeViewModel viewModel)
        {
            return viewModel.EnterWasteCodesViewModel.SelectedCode.HasValue;
        }

        protected abstract Task<ActionResult> ContinueAction(Guid id, BaseWasteCodeViewModel viewModel);
    }
}