namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.WasteCodes;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteCodes;
    using ViewModels.BaselOecdCode;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class BaselOecdCodeController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMap<WasteCodeDataAndNotificationData, BaselOecdCodeViewModel> codeMapper;
        private static readonly IList<CodeType> RequiredCodeTypes = new[] { CodeType.Basel, CodeType.Oecd };

        public BaselOecdCodeController(IMediator mediator,
            IMap<WasteCodeDataAndNotificationData, BaselOecdCodeViewModel> codeMapper)
        {
            this.mediator = mediator;
            this.codeMapper = codeMapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool backToOverview = false)
        {
            var result =
                await
                    mediator.SendAsync(new GetWasteCodeLookupAndNotificationDataByTypes(id, RequiredCodeTypes, RequiredCodeTypes));

            return View(codeMapper.Map(result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, BaselOecdCodeViewModel model, bool backToOverview = false)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var codeType = (model.GetCode().HasValue)
                ? model.WasteCodes.Single(wc => wc.Id == model.GetCode().Value).CodeType
                : CodeType.Basel;

            await
                mediator.SendAsync(new SetBaselOecdCodeForNotification(id,
                        codeType,
                        model.NotListed,
                        model.GetCode()));

            return backToOverview ? RedirectToAction("Index", "Home", new { id })
                : RedirectToAction("Index", "EwcCode", new { id });
        }
    }
}