namespace EA.Iws.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Client.Entities;
    using Infrastructure;

    [RoutePrefix("api/ErrorLog")]
    [AllowAnonymous]
    public class ErrorLogController : ApiController
    {
        private readonly ElmahSqlLogger logger;

        public ErrorLogController(ElmahSqlLogger logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create(ErrorData errorData)
        {
            await logger.Log(errorData);

            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(string id, [FromUri] string applicationName = "")
        {
            var errorData = await logger.GetError(Guid.Parse(id), applicationName);

            return Ok(errorData);
        }

        [HttpGet]
        [Route("list")]
        public async Task<IHttpActionResult> GetList([FromUri] int pageIndex, [FromUri] int pageSize, [FromUri] string applicationName = "")
        {
            var pagedErrorList = await logger.GetPagedErrorList(pageIndex, pageSize, applicationName);

            return Ok(pagedErrorList);
        }
    }
}