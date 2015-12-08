namespace EA.Iws.Web.Areas.ExportMovement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Summary(Guid id)
        {
            var result = mediator
                .SendAsync(new GetMovementSummary(id))
                .GetAwaiter()
                .GetResult();

            return PartialView("_Summary", result);
        }

        [HttpGet]
        public async Task<ActionResult> GenerateMovementDocument(Guid id)
        {
            var result = await mediator.SendAsync(new GenerateMovementDocument(id));

            return File(result.Content, MimeTypeHelper.GetMimeType(result.FileNameWithExtension),
                result.FileNameWithExtension);
        }
    }
}