namespace EA.Iws.Api.Client.Actions
{
    using System.Threading.Tasks;
    using Entities;

    public interface IRegistration
    {
        Task<ApiResponse<string>> RegisterApplicantAsync(ApplicantRegistrationData applicantRegistrationData);
    }
}