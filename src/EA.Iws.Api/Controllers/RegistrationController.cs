namespace EA.Iws.Api.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Client.Entities;

    public class RegistrationController : ApiController
    {
        [Authorize]
        public HttpResponseMessage Post(ApplicantRegistrationData applicantRegistration)
        {
            return new HttpResponseMessage {StatusCode = HttpStatusCode.OK};
        }
    }
}