namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Core.Annexes;
    using Prsd.Core.Mediator;
    using Requests.Annexes;
    using ViewModels.Annex;

    [Authorize]
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
            var result = await mediator.SendAsync(new GetAnnexesToBeProvided(id));

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
        public async Task<ActionResult> Index(Guid id, AnnexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.ProcessOfGeneration != null && model.ProcessOfGenerationStatus.IsRequired)
            {
                await
                mediator.SendAsync(
                    new SetProcessOfGenerationAnnex(new AnnexUpload(GetFileBytes(model.ProcessOfGeneration),
                        model.ProcessOfGeneration.ContentType, id)));
            }

            if (model.TechnologyEmployed != null && model.TechnologyEmployedStatus.IsRequired)
            {
                // TODO: Request and handler
            }

            if (model.Composition != null && model.WasteCompositionStatus.IsRequired)
            {
                // TODO: Request and handler
            }

            return RedirectToAction("Index", "Options");
        }

        private byte[] GetFileBytes(HttpPostedFileBase file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);

                return memoryStream.ToArray();
            }
        }
    }
}