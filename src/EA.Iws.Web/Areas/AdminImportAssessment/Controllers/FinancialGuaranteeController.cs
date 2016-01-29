namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.FinancialGuarantee;

    public class FinancialGuaranteeController : Controller
    {
        private readonly IMediator mediator;

        public FinancialGuaranteeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ActionResult> ReceivedDate(Guid id)
        {
            var receivedDate = await mediator.SendAsync(new GetReceivedDate(id));

            if (receivedDate.HasValue)
            {
                return View();
            }

            return RedirectToAction("CompletedDate");
        }

        public Task<ActionResult> CompletedDate(Guid id)
        {
            throw new NotImplementedException();
        } 
    }
}