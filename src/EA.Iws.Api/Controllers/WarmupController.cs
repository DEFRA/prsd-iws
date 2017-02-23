namespace EA.Iws.Api.Controllers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DataAccess;
    using DataAccess.Filestore;
    using DataAccess.Identity;

    [AllowAnonymous]
    [RoutePrefix("api/warmup")]
    public class WarmupController : ApiController
    {
        private readonly IwsContext iwsContext;
        private readonly ImportNotificationContext importNotificationContext;
        private readonly IwsFileStoreContext fileStoreContext;
        private readonly IwsIdentityContext identityContext;

        public WarmupController(IwsContext iwsContext, ImportNotificationContext importNotificationContext,
            IwsFileStoreContext fileStoreContext, IwsIdentityContext identityContext)
        {
            this.iwsContext = iwsContext;
            this.importNotificationContext = importNotificationContext;
            this.fileStoreContext = fileStoreContext;
            this.identityContext = identityContext;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            // Warmup Entity Framework contexts by making a query
            if (iwsContext.Database.Exists())
            {
                await iwsContext.NotificationApplications.FirstOrDefaultAsync();
            }

            if (importNotificationContext.Database.Exists())
            {
                await importNotificationContext.ImportNotifications.FirstOrDefaultAsync();
            }

            if (fileStoreContext.Database.Exists())
            {
                await fileStoreContext.Files.FirstOrDefaultAsync();
            }

            if (identityContext.Database.Exists())
            {
                await identityContext.Users.FirstOrDefaultAsync();
            }

            return Ok();
        }
    }
}