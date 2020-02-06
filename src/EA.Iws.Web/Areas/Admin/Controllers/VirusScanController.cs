namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Scanning;
    using ViewModels.VirusScan;

    [Authorize(Roles = "internal,administrator")]
    public class VirusScanController : Controller
    {
        private readonly IVirusScanner virusScanner;

        public VirusScanController(IVirusScanner virusScanner)
        {
            this.virusScanner = virusScanner;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            await virusScanner.ScanFileAsync(Encoding.ASCII.GetBytes("test"), User.GetAccessToken());
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(VirusScanViewModel model)
        {
            var timeStart = DateTime.Now;

            var fileContents = new MemoryStream();
            model.File.InputStream.CopyTo(fileContents);
            
            var result = await virusScanner.ScanFileAsync(fileContents.ToArray(), User.GetAccessToken());

            ViewBag.Message = string.Format("Started: {0} Ended: {1} Result: {2}", timeStart, DateTime.UtcNow, result.ToString());

            return View();
        }
    }
}
