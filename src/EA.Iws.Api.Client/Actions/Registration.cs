namespace EA.Iws.Api.Client.Actions
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Entities;
    using Extensions;

    internal class Registration : IRegistration
    {
        private readonly HttpClient httpClient;
        private readonly string controller = "Registration/";

        public Registration(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> RegisterApplicantAsync(ApplicantRegistrationData applicantRegistrationData)
        {
            return await httpClient.PostAsJsonAsync(controller + "Register", applicantRegistrationData);
        }

        public async Task<HttpResponseMessage> RegisterOrganisationAsync(string accessToken, OrganisationRegistrationData organisationRegistrationData)
        {
            return await httpClient.PostAsJsonAsync(accessToken, controller + "Register", organisationRegistrationData);
        }

        public async Task<OrganisationData[]> SearchOrganisationAsync(string organisationName)
        {
            OrganisationData[] organisations = await httpClient.GetAsync<OrganisationData[]>(controller + "OrganisationSearch/" + organisationName);

            return organisations;
        }
    }
}