namespace EA.Iws.Api.Client.Actions
{
    using System.Threading.Tasks;
    using Entities;

    public interface IRegistration
    {
        Task<string> RegisterApplicantAsync(string accessToken, ApplicantRegistrationData applicantRegistrationData);
        Task<string> RegisterAdminAsync(string accessToken, AdminRegistrationData adminRegistrationData);
        Task<bool> VerifyEmailAsync(string accessToken, VerifiedEmailData verifiedEmailData);
        Task<bool> SendEmailVerificationAsync(string accessToken, EmailVerificationData emailVerificationData);
        Task<EditApplicantRegistrationData> GetApplicantDetailsAsync(string accessToken);

        Task<string> UpdateApplicantDetailsAsync(string accessToken,
            EditApplicantRegistrationData applicantRegistrationData);

        Task<bool> ResetPasswordRequestAsync(string accessToken, PasswordResetRequest passwordResetRequest);

        Task<bool> ResetPasswordAsync(string accessToken, PasswordResetData passwordResetData);
    }
}