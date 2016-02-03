namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Annexes;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.Annexes;
    using ViewModels.Annex;

    [Authorize]
    public class AnnexController : Controller
    {
        private readonly IMediator mediator;
        private readonly IFileReader fileReader;

        public AnnexController(IMediator mediator, IFileReader fileReader)
        {
            this.mediator = mediator;
            this.fileReader = fileReader;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result = await mediator.SendAsync(new GetAnnexes(id));

            return View(new AnnexViewModel
            {
                NotificationId = id,
                WasteCompositionStatus = result.WasteComposition,
                TechnologyEmployedStatus = result.TechnologyEmployed,
                ProcessOfGenerationStatus = result.ProcessOfGeneration
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, AnnexViewModel model, Guid? delete)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (delete.HasValue)
            {
                await DeleteAnnex(id, delete.Value);

                return RedirectToAction("Index");
            }

            if (model.ProcessOfGeneration != null && model.ProcessOfGenerationStatus.IsRequired)
            {
                await
                mediator.SendAsync(
                    new SetProcessOfGenerationAnnex(new AnnexUpload(await fileReader.GetFileBytes(model.ProcessOfGeneration),
                        Path.GetExtension(model.ProcessOfGeneration.FileName), id)));
            }

            if (model.TechnologyEmployed != null && model.TechnologyEmployedStatus.IsRequired)
            {
                await
                    mediator.SendAsync(
                        new SetTechnologyEmployedAnnex(new AnnexUpload(await fileReader.GetFileBytes(model.TechnologyEmployed),
                            Path.GetExtension(model.TechnologyEmployed.FileName), id)));
            }

            if (model.Composition != null && model.WasteCompositionStatus.IsRequired)
            {
                await
                    mediator.SendAsync(
                        new SetWasteCompositionAnnex(new AnnexUpload(await fileReader.GetFileBytes(model.Composition),
                            Path.GetExtension(model.Composition.FileName), id)));
            }

            return RedirectToAction("Success");
        }

        [HttpGet]
        public ActionResult Success(Guid id)
        {
            var model = new SuccessViewModel{ NotificationId = id };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> ViewAnnex(Guid id, Guid fileId)
        {
            var result = await mediator.SendAsync(new GetAnnexFile(id, fileId));

            return File(result.Content, MimeTypeHelper.GetMimeType(result.Type),
                string.Format("{0}.{1}", result.Name, result.Type));
        }
        
        private async Task DeleteAnnex(Guid id, Guid fileId)
        {
            await mediator.SendAsync(new DeleteAnnexFile(id, fileId));
        }
    }
}