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

        public async Task<bool> SendEmailVerificationAsync(string accessToken, EmailVerificationData emailVerificationData)
        {
            httpClient.SetBearerToken(accessToken);
            var response = await httpClient.PostAsJsonAsync(Controller + "SendEmailVerification", emailVerificationData);
            return await response.CreateResponseAsync<bool>();
        }

        public async Task<EditApplicantRegistrationData> GetApplicantDetailsAsync(string accessToken)
        {
            httpClient.SetBearerToken(accessToken);
            var response = await httpClient.GetAsync(Controller + "GetApplicantDetails");
            return await response.CreateResponseAsync<EditApplicantRegistrationData>();
        }

        public async Task<string> UpdateApplicantDetailsAsync(string accessToken, EditApplicantRegistrationData applicantRegistrationData)
        {
            httpClient.SetBearerToken(accessToken);
            var response = await httpClient.PostAsJsonAsync(Controller + "UpdateApplicantDetails", applicantRegistrationData);
            return await response.CreateResponseAsync<string>();
        }

        public async Task<bool> ResetPasswordRequestAsync(PasswordResetRequest passwordResetRequest)
        {
            var response = await httpClient.PostAsJsonAsync(Controller + "ResetPasswordRequest", passwordResetRequest);

            return await response.CreateResponseAsync<bool>();
        }

        public async Task<bool> ResetPasswordAsync(PasswordResetData passwordResetData)
        {
            var response = await httpClient.PostAsJsonAsync(Controller + "ResetPassword", passwordResetData);

            return await response.CreateResponseAsync<bool>();
        }
    }
}