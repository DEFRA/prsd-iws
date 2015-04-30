namespace EA.Iws.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Client.Entities;
    using Core.Cqrs;
    using Cqrs.Notification;

    public class NotificationInformationController : ApiController
    {
        private readonly IQueryBus queryBus;

        public NotificationInformationController(IQueryBus queryBus)
        {
            this.queryBus = queryBus;
        }

        public async Task<IHttpActionResult> Get(Guid id)
        {
            var query = new GetNotificationNumber(id);
            var number = await queryBus.QueryAsync(query);

            var data = new NotificationInformation(id, number);

            return Ok(data);
        }
    }
}