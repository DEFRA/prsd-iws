namespace EA.Iws.Api.Client.Actions
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Entities;
    using Extensions;

    internal class ApplicantRegistration : IApplicantRegistration
    {
        private readonly HttpClient httpClient;

        public ApplicantRegistration(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> RegisterApplicantAsync(string accessToken, ApplicantRegistrationData applicantRegistrationData)
        {
            return await httpClient.PostProtectedAsync<ApplicantRegistrationData>(accessToken, "Registration/Register", applicantRegistrationData);
        }
    }
}