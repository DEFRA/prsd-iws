namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Annexes;
    using Requests.Files;
    using ViewModels.Annex;

    [AuthorizeActivity(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class AnnexController : Controller
    {
        private readonly IMediator mediator;

        public AnnexController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result = await mediator.SendAsync(new GetAnnexes(id));

            return View(new DownloadAnnexViewModel(result));
        }

        [HttpGet]
        public async Task<ActionResult> Download(Guid id, Guid fileId)
        {
            var result = await mediator.SendAsync(new GetFile(id, fileId));

            return File(result.Content, MimeTypeHelper.GetMimeType(result.Type),
                string.Format("{0}.{1}", result.Name, result.Type));
        }
    }
}