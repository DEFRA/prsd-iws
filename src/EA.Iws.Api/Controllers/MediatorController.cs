namespace EA.Iws.Api.Controllers
{
    using System;
    using System.Net;
    using System.Security;
    using System.Security.Authentication;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Core.Authorization;
    using Infrastructure;
    using Newtonsoft.Json;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Converters;
    using Serilog;

    [RoutePrefix("api")]
    public class MediatorController : ApiController
    {
        private readonly IMediator mediator;
        private readonly ILogger logger;

        public MediatorController(IMediator mediator, ILogger logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        /// <summary>
        ///     This API action fills the role of the mediator. It receives messages and sends responses.
        /// </summary>
        /// <param name="apiRequest">The wrapped request object with assembly qualified type name.</param>
        /// <returns>An object of the correct type (which will be serialized as json).</returns>
        [HttpPost]
        [Route("Send")]
        public async Task<IHttpActionResult> Send(ApiRequest apiRequest)
        {
            try
            {
                var typeInformation = new RequestTypeInformation(apiRequest);

                var result = JsonConvert.DeserializeObject(apiRequest.RequestJson, typeInformation.RequestType,
                    new EnumerationConverter());

                var response = await mediator.SendAsync(result, typeInformation.ResponseType);
                return Ok(response);
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
            catch (RequestAuthorizationException ex)
            {
                logger.Error(ex, "Request authorization error");
                return this.StatusCode(HttpStatusCode.Forbidden, new HttpError(ex, includeErrorDetail: true));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Unhandled error");
                throw;
            }
        }

        private class RequestTypeInformation
        {
            public RequestTypeInformation(ApiRequest apiRequest)
            {
                RequestType = Type.GetType(apiRequest.TypeName);

                if (RequestType == null)
                {
                    throw new InvalidOperationException(
                        "The passed type name could not be resolved to a type! Type name: " + apiRequest.TypeName);
                }

                ResponseType = RequestType.GetInterfaces()[0].GenericTypeArguments[0];
            }

            public Type RequestType { get; private set; }

            public Type ResponseType { get; private set; }
        }
    }
}