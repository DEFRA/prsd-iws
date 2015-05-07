namespace EA.Iws.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Client.Entities;
    using Core.Cqrs;
    using Core.Domain;
    using Cqrs.Notification;
    using Identity;

    [RoutePrefix("api/Producer")]
    public class ProducerController : ApiController
    {
        private readonly ICommandBus commandBus;

        public ProducerController(ICommandBus commandBus)
        {
            this.commandBus = commandBus;
        }

        [HttpPost]
        [Authorize]
        [Route("Create")]
        public async Task<IHttpActionResult> Create(ProducerData producer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await commandBus.SendAsync(new CreateProducer(producer));

            return Ok(producer.Name);
        }
    }
}