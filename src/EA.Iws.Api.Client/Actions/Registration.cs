namespace EA.Iws.Api.Client.Actions
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Entities;
    using Extensions;

    internal class Registration : IRegistration
    {
        private readonly HttpClient httpClient;

        public Registration(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> RegisterApplicantAsync(ApplicantRegistrationData applicantRegistrationData)
        {
            return await httpClient.PostAsJsonAsync<ApplicantRegistrationData>("Registration/Register", applicantRegistrationData);
        }

        public async Task<HttpResponseMessage> RegisterOrganisationAsync(string accessToken, OrganisationRegistrationData organisationRegistrationData)
        {
            return await httpClient.PostProtectedAsync<OrganisationRegistrationData>(accessToken, "Registration/Register", organisationRegistrationData);
        }
    }
}