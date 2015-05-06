namespace EA.Iws.Api.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using Core.Cqrs;
    using Cqrs.Notification;

    public class DocumentGenerationController : ApiController
    {
        private readonly IQueryBus queryBus;

        public DocumentGenerationController(IQueryBus queryBus)
        {
            this.queryBus = queryBus;
        }

        public async Task<HttpResponseMessage> Get(Guid id)
        {
            var query = new GenerateNotificationDocument(id, HttpRuntime.AppDomainAppPath + "Documents\\");

            var documentByteArray = await queryBus.QueryAsync(query);

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(documentByteArray);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }
    }
}