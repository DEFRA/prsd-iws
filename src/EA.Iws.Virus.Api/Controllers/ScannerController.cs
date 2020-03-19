namespace EA.Iws.Virus.Api.Controllers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Security;
    using System.Security.Authentication;
    using System.Threading.Tasks;
    using System.Web.Http;
    using IWS.Api.Infrastructure.Infrastructure;
    using Scanning;

    [RoutePrefix("api/scanner")]
    [Authorize]
    public class ScannerController : ApiController
    {
        private readonly IVirusScanner virusScanner;

        public ScannerController(IVirusScanner virusScanner)
        {
            this.virusScanner = virusScanner;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Scan")]
        public IHttpActionResult Scan()
        {
            return Ok("ok");
        }

        [HttpPost]
        [Route("Scan")]
        public async Task<IHttpActionResult> Scan(byte[] file)
        {
            try
            {
                var result = await virusScanner.ScanFileAsync(file, string.Empty);

                return Ok(result);
            }
            catch (AuthenticationException ex)
            {
                return this.StatusCode(HttpStatusCode.Unauthorized, new HttpError(ex, includeErrorDetail: true));
            }
            catch (SecurityException ex)
            {
                return this.StatusCode(HttpStatusCode.Forbidden, new HttpError(ex, includeErrorDetail: true));
            }
            catch (IOException ex)
            {
                return this.StatusCode(HttpStatusCode.InternalServerError, new HttpError(ex, includeErrorDetail: true));
            }
        }
    }
}
