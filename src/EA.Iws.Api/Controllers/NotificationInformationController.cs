namespace EA.Iws.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Client.Entities;
    using Core.Cqrs;
    using Core.Domain;
    using Cqrs.Notification;

    [Authorize]
    [RoutePrefix("api/NotificationInformation")]
    public class NotificationInformationController : ApiController
    {
        private readonly IQueryBus queryBus;
        private readonly IUserContext userContext;

        public NotificationInformationController(IQueryBus queryBus, IUserContext userContext)
        {
            this.queryBus = queryBus;
            this.userContext = userContext;
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IHttpActionResult> GetById(Guid id)
        {
            var query = new GetNotificationNumber(id);
            var number = await queryBus.QueryAsync(query);

            var data = new NotificationInformation(id, number);

            return Ok(data);
        }

        [HttpGet]
        [Route("GetUserNotifications")]
        public async Task<IHttpActionResult> GetUserNotifications()
        {
            NotificationApplicationSummaryData[] model = null;

            var notificationApplications = await queryBus.QueryAsync(new GetNotificationsByUser(userContext.UserId));

            if (notificationApplications == null)
            {
                return Ok(model);
            }

            model = new NotificationApplicationSummaryData[notificationApplications.Count];

            for (int i = 0; i < notificationApplications.Count; i++)
            {
                model[i] = new NotificationApplicationSummaryData
                {
                    Id = notificationApplications[i].Id,
                    NotificationNumber = notificationApplications[i].NotificationNumber,
                    CreatedDate = notificationApplications[i].CreatedDate
                };
            }

            return Ok(model);
        }
    }
}