namespace EA.Iws.Api.Client.Actions
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Entities;
    using Prsd.Core.Web.Extensions;

    internal class Registration : IRegistration
    {
        private const string Controller = "Registration/";
        private readonly HttpClient httpClient;

        public Registration(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> RegisterApplicantAsync(ApplicantRegistrationData applicantRegistrationData)
        {
            var response = await httpClient.PostAsJsonAsync(Controller + "Register", applicantRegistrationData);
            return await response.CreateResponseAsync<string>();
        }

        public async Task<string> RegisterAdminAsync(AdminRegistrationData adminRegistrationData)
        {
            var response = await httpClient.PostAsJsonAsync(Controller + "RegisterAdmin", adminRegistrationData);
            return await response.CreateResponseAsync<string>();
        }

        public async Task<bool> VerifyEmailAsync(VerifiedEmailData verifiedEmailData)
        {
            var response = await httpClient.PostAsJsonAsync(Controller + "VerifyEmail", verifiedEmailData);

            return await response.CreateResponseAsync<bool>();
        }

        public async Task<string> GetUserEmailVerificationTokenAsync(string accessToken)
        {
            httpClient.SetBearerToken(accessToken);

            var response = await httpClient.GetAsync(Controller + "GetUserEmailVerificationToken");

            return await response.CreateResponseAsync<string>();
        }
    }
}