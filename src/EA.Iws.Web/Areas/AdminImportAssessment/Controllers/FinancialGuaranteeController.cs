namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;

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
        }
    }
}