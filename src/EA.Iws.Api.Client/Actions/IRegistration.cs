namespace EA.Iws.Api.Client.Actions
{
    using System.Threading.Tasks;
    using Entities;

    public interface IRegistration
    {
        Task<string> RegisterApplicantAsync(ApplicantRegistrationData applicantRegistrationData);
        Task<string> RegisterAdminAsync(AdminRegistrationData adminRegistrationData);
        Task<bool> VerifyEmailAsync(VerifiedEmailData verifiedEmailData);
        Task<bool> SendEmailVerificationAsync(string accessToken, EmailVerificationData emailVerificationData);
        Task<EditApplicantRegistrationData> GetApplicantDetailsAsync(string accessToken);

        Task<string> UpdateApplicantDetailsAsync(string accessToken,
            EditApplicantRegistrationData applicantRegistrationData);

        Task<bool> ResetPasswordRequestAsync(PasswordResetRequest passwordResetRequest);

        Task<bool> ResetPasswordAsync(PasswordResetData passwordResetData);
    }
}