namespace EA.Iws.Web.Areas.FinancialGuarantee.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.FinancialGuarantee;

    public class HomeController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public HomeController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> GenerateFinancialGuaranteeDocument()
        {
            using (var client = apiClient())
            {
                try
                {
                    var response =
                        await client.SendAsync(User.GetAccessToken(), new GenerateFinancialGuaranteeDocument());

                    var downloadName = "IwsFinancialGuarantee" + SystemTime.UtcNow + ".pdf";

                    return File(response, "application/pdf", downloadName);
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);
                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                    return HttpNotFound();
                }
            }
        }
    }
}