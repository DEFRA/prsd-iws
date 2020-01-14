namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.VirusScanning;
    using ViewModels.VirusScan;

    [Authorize(Roles = "internal,readonly")]
    public class VirusScanController : Controller
    {
        private readonly IVirusScanner virusScanner;

        public VirusScanController(IVirusScanner virusScanner)
        {
            this.virusScanner = virusScanner;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(VirusScanViewModel model)
        {
            var timeStart = DateTime.Now;

            var fileContents = new MemoryStream();
            model.File.InputStream.CopyTo(fileContents);
            
            var result = await virusScanner.ScanFileAsync(fileContents.ToArray());

            ViewBag.Message = $"Started: {timeStart} Ended: {DateTime.Now} Result: {result.ToString()}";

            return View();
        }
    }
}
