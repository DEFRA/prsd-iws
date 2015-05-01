namespace EA.Iws.Api.Client.Actions
{
    using System;
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

        public async Task<Response<string>> RegisterApplicantAsync(ApplicantRegistrationData applicantRegistrationData)
        {
            var response = await httpClient.PostAsJsonAsync(controller + "Register", applicantRegistrationData);
            return await response.CreateResponseAsync<string>();
        }

        public async Task<Response<string>> RegisterOrganisationAsync(string accessToken, OrganisationRegistrationData organisationRegistrationData)
        {
            var response = await httpClient.PostAsJsonAsync(accessToken, controller + "RegisterOrganisation", organisationRegistrationData);
            return await response.CreateResponseAsync<string>();
        }

        public async Task<OrganisationData[]> SearchOrganisationAsync(string accessToken, string organisationName)
        {
            organisationName = organisationName.Replace(".", string.Empty);

            OrganisationData[] organisations = await httpClient.GetAsync<OrganisationData[]>(accessToken, controller + "OrganisationSearch/" + organisationName);

            return organisations;
        }

        public async Task<HttpResponseMessage> LinkUserToOrganisationAsync(string accessToken, Guid organisationId)
        {
            return await httpClient.PostAsJsonAsync(accessToken, controller + "OrganisationSelect", new OrganisationLinkData{ OrganisationId = organisationId});
        }

        public async Task<CountryData[]> GetCountriesAsync()
        {
            return await httpClient.GetAsync<CountryData[]>("Registration/GetCountries");
        }
    }
}