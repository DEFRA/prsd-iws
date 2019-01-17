namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.Notification.Audit;
    using Core.WasteCodes;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.WasteCodes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.WasteCodes;
    using Views.Shared;

    [Authorize]
    [NotificationReadOnlyFilter]
    public abstract class BaseWasteCodeController : Controller
    {
        protected const string AddCode = "addcode";
        protected const string Continue = "continue";
        protected readonly IMediator Mediator;
        protected readonly IAuditService AuditService;
        private readonly CodeType codeType;

        protected BaseWasteCodeController(IMediator mediator, CodeType codeType, IAuditService auditService)
        {
            this.Mediator = mediator;
            this.codeType = codeType;
            this.AuditService = auditService;
        }

        public async Task<ActionResult> Post(Guid id, BaseWasteCodeViewModel viewModel, string command, string remove, bool backToOverview)
        {
            await RebindModel(id, viewModel);

            if (!string.IsNullOrWhiteSpace(remove))
            {
                ModelState.Clear();

                viewModel.EnterWasteCodesViewModel.SelectedWasteCodes.RemoveAll(c => c.ToString().Equals(remove));

                return RemoveAction(viewModel);
            }
            
            if (string.IsNullOrWhiteSpace(command) && string.IsNullOrWhiteSpace(remove))
            {
                throw new InvalidOperationException();
            }

            if (!ModelState.IsValid || (command.Equals(AddCode) && !HasASelectedCode(viewModel)))
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

                return await ContinueAction(id, viewModel, backToOverview);
            }

            throw new InvalidOperationException();
        }

        protected virtual async Task RebindModel(Guid id, BaseWasteCodeViewModel viewModel)
        {
            var result =
                await
                    Mediator.SendAsync(new GetWasteCodeLookupAndNotificationDataByTypes(id, new[] { codeType }));

            viewModel.EnterWasteCodesViewModel.WasteCodes =
                result.LookupWasteCodeData[codeType].Select(wc => new WasteCodeViewModel
                {
                    Id = wc.Id,
                    CodeType = wc.CodeType,
                    Description = wc.Description,
                    Name = wc.Code
                }).ToList();
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

        protected abstract Task<ActionResult> ContinueAction(Guid id, BaseWasteCodeViewModel viewModel, bool backToOverview);

        protected ActionResult BackToOverviewResult(Guid id)
        {
            return RedirectToAction("Index", "Home", new { id });
        }

        protected bool CodeBeenRemoved(BaseWasteCodeViewModel model, WasteCodeDataAndNotificationData existingData)
        {
            foreach (var code in existingData.NotificationWasteCodeData[this.codeType])
            {
                if (model.EnterWasteCodesViewModel.SelectedWasteCodes.Count(p => p == code.Id) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        protected bool CodeBeenAdded(BaseWasteCodeViewModel model, WasteCodeDataAndNotificationData existingData)
        {
            foreach (var code in model.EnterWasteCodesViewModel.SelectedWasteCodes)
            {
                if (existingData.NotificationWasteCodeData[this.codeType].Count(p => p.Id == code) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        protected async Task AddAuditEntries(WasteCodeDataAndNotificationData existingData, BaseWasteCodeViewModel viewModel, Guid id, NotificationAuditScreenType screenType)
        {
            if (!existingData.NotificationWasteCodeData[codeType].Any())
            {
                await AuditService.AddAuditEntry(Mediator,
                   id,
                   User.GetUserId(),
                   NotificationAuditType.Create,
                   screenType);
            }
            else
            {
                if (CodeBeenRemoved(viewModel, existingData))
                {
                    await AuditService.AddAuditEntry(Mediator,
                       id,
                       User.GetUserId(),
                        NotificationAuditType.Delete,
                       screenType);
                }

                if (CodeBeenAdded(viewModel, existingData))
                {
                    await AuditService.AddAuditEntry(Mediator,
                       id,
                       User.GetUserId(),
                        NotificationAuditType.Update,
                       screenType);
                }
            }
        }
    }
}