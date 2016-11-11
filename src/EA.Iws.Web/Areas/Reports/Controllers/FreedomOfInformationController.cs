namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.Reports;
    using Core.WasteType;
    using Infrastructure;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;
    using ViewModels.FreedomOfInformation;

    [Authorize(Roles = "internal")]
    public class FreedomOfInformationController : Controller
    {
        private readonly IMediator mediator;

        public FreedomOfInformationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new IndexViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Download", new
            {
                From = model.FromDate.AsDateTime().Value,
                To = model.ToDate.AsDateTime().Value,
                model.ChemicalComposition
            });
        }

        [HttpGet]
        public async Task<ActionResult> Download(DateTime from, DateTime to, ChemicalComposition chemicalComposition)
        {
            var report = await mediator.SendAsync(new GetFreedomOfInformationReport(from, to, chemicalComposition));

            var type = EnumHelper.GetShortName(chemicalComposition);

            var fileName = string.Format("{0}-{1}-{2}.csv", type, from, to);

            return new CsvActionResult<FreedomOfInformationData>(report.ToList(), fileName);
        }
    }
}