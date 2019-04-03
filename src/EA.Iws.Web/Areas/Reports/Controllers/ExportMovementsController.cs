namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.Reports;
    using EA.Iws.Core.Reports;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;
    using ViewModels.ExportMovements;

    [AuthorizeActivity(typeof(GetExportMovementsReport))]
    public class ExportMovementsController : Controller
    {
        private readonly IMediator mediator;

        public ExportMovementsController(IMediator mediator)
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
        public async Task<ActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var from = model.From.AsDateTime().Value;
            var to = model.To.AsDateTime().Value.AddDays(1);

            var organisationType = ReportEnumParser.TryParse<OrganisationFilterOptions>(model.SelectedOrganistationFilter);

            string organisationName = organisationType == null ? null : model.OrganisationName;

            var report = await mediator.SendAsync(new GetExportMovementsReport(from, to, organisationType, organisationName));

            var filename = string.Format("export-movement-documents-input-{0}-{1}.xlsx",
                from.ToShortDateString(),
                to.ToShortDateString());

            return new XlsxActionResult<ExportMovementsData>(new[] { report }, filename);
        }
    }
}