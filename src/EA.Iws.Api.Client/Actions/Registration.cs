namespace EA.Iws.Api.Client.Actions
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Entities;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Extensions;

    internal class Registration : IRegistration
    {
        private readonly HttpClient httpClient;
        private readonly string controller = "Registration/";

        public Registration(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ApiResponse<string>> RegisterApplicantAsync(ApplicantRegistrationData applicantRegistrationData)
        {
            var response = await httpClient.PostAsJsonAsync(controller + "Register", applicantRegistrationData);
            return await response.CreateResponseAsync<string>();
        }
    }
}