namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.Admin;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Requests.Admin.FinancialGuarantee;
    using ViewModels.FinancialGuarantee;

    [Authorize(Roles = "internal")]
    public class FinancialGuaranteeController : Controller
    {
        private readonly Func<IIwsClient> apiClient;
        private readonly IMap<FinancialGuaranteeData, FinancialGuaranteeDatesViewModel> dateMap;
        private readonly IMap<FinancialGuaranteeData, FinancialGuaranteeDecisionViewModel> decisionMap;
        private readonly IMapWithParameter<FinancialGuaranteeDecisionViewModel, Guid, FinancialGuaranteeDecisionRequest> requestMap;

        public FinancialGuaranteeController(Func<IIwsClient> apiClient,
            IMap<FinancialGuaranteeData, FinancialGuaranteeDatesViewModel> dateMap,
            IMap<FinancialGuaranteeData, FinancialGuaranteeDecisionViewModel> decisionMap,
            IMapWithParameter<FinancialGuaranteeDecisionViewModel, Guid, FinancialGuaranteeDecisionRequest> requestMap)
        {
            this.apiClient = apiClient;
            this.dateMap = dateMap;
            this.decisionMap = decisionMap;
            this.requestMap = requestMap;
        }

        [HttpGet]
        public async Task<ActionResult> Dates(Guid id)
        {
            using (var client = apiClient())
            {
                var result = await
                    client.SendAsync(User.GetAccessToken(),
                        new GetFinancialGuaranteeDataByNotificationApplicationId(id));

                return View(dateMap.Map(result));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Dates(Guid id, FinancialGuaranteeDatesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                await
                    client.SendAsync(User.GetAccessToken(), new SetFinancialGuaranteeDates(id, model.Received.AsDateTime(),
                        model.Completed.AsDateTime()));

                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
        }

        [HttpGet]
        public async Task<ActionResult> Decision(Guid id)
        {
            using (var client = apiClient())
            {
                var result = await client.SendAsync(User.GetAccessToken(),
                    new GetFinancialGuaranteeDataByNotificationApplicationId(id));

                return View(decisionMap.Map(result));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Decision(Guid id, FinancialGuaranteeDecisionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), requestMap.Map(model, id));

                return RedirectToAction("Decision", new { id });
            }
        }
    }
}