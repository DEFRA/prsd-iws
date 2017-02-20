namespace EA.Iws.Api.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using Client.Entities;

    [RoutePrefix("api/ErrorLog")]
    [AllowAnonymous]
    public class ErrorLogController : ApiController
    {
        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create(ErrorData errorData)
        {
            await Task.Yield();
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            await Task.Yield();
            return Ok(new ErrorData());
        }

        [HttpGet]
        [Route("list")]
        public async Task<IHttpActionResult> GetList([FromUri] int pageIndex, [FromUri] int pageSize)
        {
            await Task.Yield();
            return Ok(new PagedErrorDataList
            {
                Errors = new ErrorData[] { }
            });
        }
    }
}