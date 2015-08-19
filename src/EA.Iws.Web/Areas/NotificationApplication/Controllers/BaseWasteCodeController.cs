namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using ViewModels.WasteCodes;

    public abstract class BaseWasteCodeController : Controller
    {
        protected const string AddCode = "addcode";
        protected const string Continue = "continue";
        protected readonly Func<IIwsClient> apiClient;

        protected BaseWasteCodeController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ActionResult> Post(BaseWasteCodeViewModel viewModel, string command, string remove)
        {
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

            if (command.Equals(AddCode) && viewModel.EnterWasteCodesViewModel.SelectedCode.HasValue)
            {
                AddCodeToViewModel(viewModel);

                return AddAction(viewModel);
            }

            if (command.Equals(Continue) && viewModel.EnterWasteCodesViewModel.SelectedCode.HasValue)
            {
                AddCodeToViewModel(viewModel);

                return await ContinueAction(viewModel);
            }

            throw new NotImplementedException();
        }

        private void AddCodeToViewModel(BaseWasteCodeViewModel viewModel)
        {
            if (viewModel.EnterWasteCodesViewModel.SelectedWasteCodes == null)
            {
                viewModel.EnterWasteCodesViewModel.SelectedWasteCodes = new List<Guid>();
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

        protected abstract Task<ActionResult> ContinueAction(BaseWasteCodeViewModel viewModel);
    }
}