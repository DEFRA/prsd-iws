namespace EA.Iws.Web.Areas.MovementDocument.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Movement;

    [Authorize]
    public class QuantityController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public QuantityController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> ActionResult(Guid id)
        {
            using (var client = apiClient())
            {
                var result = await client.SendAsync(User.GetAccessToken(), new GetMovementQuantityDataByMovementId(id));

                throw new NotImplementedException();
            }
        }
    }
}