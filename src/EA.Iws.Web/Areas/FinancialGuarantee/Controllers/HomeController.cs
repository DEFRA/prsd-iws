namespace EA.Iws.Web.Areas.FinancialGuarantee.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Notification;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.FinancialGuarantee;

    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GenerateBankGuaranteeDocument()
        {
            try
            {
                var response = await mediator.SendAsync(new GenerateBankGuaranteeDocument());

                var downloadName = "IwsBankGuarantee" + SystemTime.UtcNow + ".doc";

                return File(response, "application/msword", downloadName);
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

        [HttpGet]
        public async Task<ActionResult> GenerateParentCompanyDocument()
        {
            try
            {
                var response = await mediator.SendAsync(new GenerateParentCompanyDocument());

                var downloadName = "IwsParentCompanyGuarantee" + SystemTime.UtcNow + ".doc";

                return File(response, "application/msword", downloadName);
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

        [HttpGet]
        public async Task<ActionResult> GenerateFinancialGuaranteeDocument(CompetentAuthority competentAuthority)
        {
            try
            {
                var response = await mediator.SendAsync(new GenerateFinancialGuaranteeDocument(competentAuthority));

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