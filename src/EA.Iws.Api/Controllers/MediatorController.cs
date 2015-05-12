namespace EA.Iws.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Client;
    using Newtonsoft.Json;
    using Prsd.Core.Mediator;

    [RoutePrefix("api")]
    public class MediatorController : ApiController
    {
        private readonly IMediator mediator;

        public MediatorController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// This API action fills the role of the mediator. It receives messages and sends responses.
        /// </summary>
        /// <param name="apiRequest">The wrapped request object with assembly qualified type name.</param>
        /// <returns>An object of the correct type (which will be serialized as json).</returns>
        [HttpPost]
        [Route("Send")]
        public async Task<object> Send(ApiRequest apiRequest)
        {
            var typeInformation = new RequestTypeInformation(apiRequest);

            var result = JsonConvert.DeserializeObject(apiRequest.RequestJson, typeInformation.RequestType);

            return await mediator.SendAsync(result, typeInformation.ResponseType);
        }

        private class RequestTypeInformation
        {
            public Type RequestType { get; private set; }

            public Type ResponseType { get; private set; }

            public RequestTypeInformation(ApiRequest apiRequest)
            {
                this.RequestType = Type.GetType(apiRequest.TypeName);

                if (RequestType == null)
                {
                    throw new InvalidOperationException("The passed type name could not be resolved to a type! Type name: " + apiRequest.TypeName);
                }

                this.ResponseType = RequestType.GetInterfaces()[0].GenericTypeArguments[0];
            } 
        }
    }
}