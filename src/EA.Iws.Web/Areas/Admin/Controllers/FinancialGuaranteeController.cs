namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Admin.FinancialGuarantee;
    using ViewModels.FinancialGuarantee;

    [Authorize(Roles = "internal")]
    public class FinancialGuaranteeController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public FinancialGuaranteeController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Dates(Guid id)
        {
            using (var client = apiClient())
            {
                var result = await
                    client.SendAsync(User.GetAccessToken(),
                        new GetFinancialGuaranteeDataByNotificationApplicationId(id));

                return View(new FinancialGuaranteeInformationViewModel(result));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Dates(Guid id, FinancialGuaranteeInformationViewModel model)
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
    }
}