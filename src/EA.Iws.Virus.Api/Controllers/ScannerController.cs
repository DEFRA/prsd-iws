namespace EA.Iws.Virus.Api.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Security;
    using System.Security.Authentication;
    using System.Threading.Tasks;
    using System.Web.Http;
    using IWS.Api.Infrastructure.Infrastructure;
    using Scanning;
    using Serilog;

    [RoutePrefix("api/scanner")]
    [Authorize]
    public class ScannerController : ApiController
    {
        private readonly ILogger logger;
        private readonly IVirusScanner virusScanner;

        public ScannerController(IVirusScanner virusScanner, ILogger logger)
        {
            this.virusScanner = virusScanner;
            this.logger = logger;
        }

        [HttpPost]
        [Route("Scan")]
        public async Task<IHttpActionResult> Scan(byte[] file)
        {
            try
            {
                var result = await virusScanner.ScanFileAsync(file);
                
                return Ok(result);
            }
            catch (AuthenticationException ex)
            {
                logger.Error(ex, "Authentication error");
                return this.StatusCode(HttpStatusCode.Unauthorized, new HttpError(ex, includeErrorDetail: true));
            }
            catch (SecurityException ex)
            {
                logger.Error(ex, "Authorization error");
                return this.StatusCode(HttpStatusCode.Forbidden, new HttpError(ex, includeErrorDetail: true));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Unhandled error");
                throw;
            }
        }
    }
}
